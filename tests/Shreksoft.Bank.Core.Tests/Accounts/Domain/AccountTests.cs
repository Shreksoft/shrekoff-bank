using Shreksoft.Bank.Core.Domain.Accounts;
using Shreksoft.Bank.Core.Domain.Accounts.Exceptions;
using Shreksoft.Bank.Core.Domain.Shared.Currencies;

namespace Shreksoft.Bank.Core.Tests.Accounts.Domain;

public class AccountTests
{
    private static Account CreateAccount(CurrencyCode currencyCode = CurrencyCode.SLP)
    {
        return new Account(Guid.NewGuid(), new Money(new Currency(currencyCode), 0));
    }

    [Fact]
    public void Constructor_InvalidCurrency_Throws()
    {
        Assert.Throws<ArgumentException>(() => new Account(Guid.NewGuid(), new Money(default, 0)));
    }

    [Fact]
    public void Debit_SufficientBalance_DecreasesBalance()
    {
        const byte amount = 100;
        var acc = CreateAccount();

        var money = new Money(acc.Money.Currency, amount);
        acc.Credit(money);
        acc.Debit(money);

        Assert.Equal(0, acc.Money.Amount);
    }

    [Fact]
    public void Debit_InsufficientBalance_Throws()
    {
        var acc = CreateAccount();

        var money = new Money(acc.Money.Currency, 100);
        Assert.Throws<InsufficientFundsException>(() => acc.Debit(money));
    }

    [Fact]
    public void Debit_AccountBlocked_Throws()
    {
        var acc = CreateAccount();
        acc.Block();

        var money = new Money(acc.Money.Currency, 100);
        Assert.Throws<AccountBlockedException>(() => acc.Debit(money));
    }

    [Fact]
    public void Debit_AmountIsNegative_Throws()
    {
        var acc = CreateAccount();
        const short amount = -1;

        var money = new Money(acc.Money.Currency, amount);
        Assert.Throws<AmountIsNegativeException>(() => acc.Debit(money));
    }

    [Fact]
    public void Credit_AccountBlocked_Throws()
    {
        var id = Guid.NewGuid();
        var acc = new Account(id, new Money(new Currency(CurrencyCode.SLP), 0));

        acc.Block();

        var money = new Money(acc.Money.Currency, 100);
        Assert.Throws<AccountBlockedException>(() => acc.Credit(money));
    }

    [Fact]
    public void Credit_AmountIsNegative_Throws()
    {
        var acc = CreateAccount();
        const short amount = -1;

        var money = new Money(acc.Money.Currency, amount);
        Assert.Throws<AmountIsNegativeException>(() => acc.Credit(money));
    }

    [Fact]
    public async Task Debit_WhenCalledConcurrentlyTotalExceedsBalance_BalanceIsPositive()
    {
        var account = CreateAccount();
        var money = new Money(account.Money.Currency, 100);
        account.Credit(money);
        const byte count = 10;
        const short debitAmount = 20;
        var debitMoney = new Money(account.Money.Currency, debitAmount);
        var tasks = new Task[count];

        for (var i = 0; i < count; i++)
            tasks[i] = Task.Run(() => account.Debit(debitMoney));

        await Assert.ThrowsAsync<InsufficientFundsException>(async () => await Task.WhenAll(tasks));
        Assert.True(account.Money.Amount >= 0);
    }

    [Fact]
    public async Task Debit_WhenCalledConcurrentlyTotalNotExceedsBalance_CorrectBalance()
    {
        var account = CreateAccount();
        var money = new Money(account.Money.Currency, 100);
        account.Credit(money);
        const byte count = 4;
        const byte debitAmount = 20;
        var debitMoney = new Money(account.Money.Currency, debitAmount);
        var tasks = new Task[count];

        for (var i = 0; i < count; i++)
            tasks[i] = Task.Run(() => account.Debit(debitMoney));

        await Task.WhenAll(tasks);

        Assert.Equal(20, account.Money.Amount);
    }
}
