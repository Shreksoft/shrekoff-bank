using CBS.Core.Accounts.Domain;
using CBS.Core.Accounts.Domain.Currencies;
using CBS.Core.Accounts.Domain.Exceptions;

namespace CBS.Core.Tests.Accounts.Domain;

public class AccountTests
{
  private static Account CreateAccount(CurrencyCode currencyCode = CurrencyCode.SLP)
  {
    return new Account(Guid.NewGuid(), new Money(new Currency(currencyCode), 0));
  }

  [Fact]
  public void Debit_SufficientBalance_DecreasesBalance()
  {
    const byte amount = 100;
    var acc = CreateAccount();

    acc.Credit(amount);
    acc.Debit(amount);

    Assert.Equal(0, acc.Money.Amount);
  }

  [Fact]
  public void Debit_InsufficientBalance_Throws()
  {
    var acc = CreateAccount();

    Assert.Throws<InsufficientFundsException>(() => acc.Debit(100));
  }

  [Fact]
  public void Debit_AccountBlocked_Throws()
  {
    var acc = CreateAccount();
    acc.Block();

    Assert.Throws<AccountBlockedException>(() => acc.Debit(100));
  }

  [Fact]
  public void Debit_AmountIsNegative_Throws()
  {
    var acc = CreateAccount();
    const short amount = -1;

    Assert.Throws<AmountIsNegativeException>(() => acc.Debit(amount));
  }

  [Fact]
  public void Credit_AccountBlocked_Throws()
  {
    var id = Guid.NewGuid();
    var acc = new Account(id, new Money(new Currency(CurrencyCode.SLP), 0));

    acc.Block();

    Assert.Throws<AccountBlockedException>(() => acc.Credit(100));
  }

  [Fact]
  public void Credit_AmountIsNegative_Throws()
  {
    var acc = CreateAccount();
    const short amount = -1;

    Assert.Throws<AmountIsNegativeException>(() => acc.Credit(amount));
  }

  [Fact]
  public async Task Debit_WhenCalledConcurrentlyTotalExceedsBalance_BalanceIsPositive()
  {
    var account = CreateAccount();
    account.Credit(100);
    const byte count = 10;
    const short debitAmount = 20;
    var tasks = new Task[count];

    for (var i = 0; i < count; i++)
      tasks[i] = Task.Run(() => account.Debit(debitAmount));

    await Assert.ThrowsAsync<InsufficientFundsException>(async () => await Task.WhenAll(tasks));
    Assert.True(account.Money.Amount >= 0);
  }

  [Fact]
  public async Task Debit_WhenCalledConcurrentlyTotalNotExceedsBalance_CorrectBalance()
  {
    var account = CreateAccount();
    account.Credit(100);
    const byte count = 4;
    const byte debitAmount = 20;
    var tasks = new Task[count];

    for (var i = 0; i < count; i++)
      tasks[i] = Task.Run(() => account.Debit(debitAmount));

    await Task.WhenAll(tasks);

    Assert.Equal(20, account.Money.Amount);
  }
}
