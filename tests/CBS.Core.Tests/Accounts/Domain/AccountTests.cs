using CBS.Core.Accounts.Domain;
using CBS.Core.Accounts.Domain.Exceptions;

namespace CBS.Core.Tests.Accounts.Domain;

public class AccountTests
{
  private Account CreateAccount(Currency currency = Currency.SLP) => new(Guid.NewGuid(), currency);

  [Fact]
  public void Debit_SufficientBalance_DecreasesBalance()
  {
    var amount = 100;
    var acc = CreateAccount();

    acc.Credit(amount);
    acc.Debit(amount);

    Assert.Equal(0, acc.Balance);
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
    var amount = -1;

    Assert.Throws<AmountIsNegativeException>(() => acc.Debit(amount));

  }

  [Fact]
  public void Credit_AccountBlocked_Throws()
  {
    var id = Guid.NewGuid();
    var acc = new Account(id, Currency.SLP);

    acc.Block();

    Assert.Throws<AccountBlockedException>(() => acc.Credit(100));
  }

  [Fact]
  public void Credit_AmountIsNegative_Throws()
  {
    var acc = CreateAccount();
    var amount = -1;

    Assert.Throws<AmountIsNegativeException>(() => acc.Credit(amount));
  }

  [Fact]
  public void EnsureSameCurrency_CurrenciesTheSame_Void()
  {
    var acc1 = CreateAccount();
    var acc2 = CreateAccount();

    acc1.EnsureSameCurrency(acc2);
  }

  [Fact]
  public void EnsureSameCurrency_CurrenciesTheDifferent_Throws()
  {
    var acc1 = CreateAccount();
    var acc2 = CreateAccount(Currency.PIZ);

    Assert.Throws<CurrencyMismatchException>(() => acc1.EnsureSameCurrency(acc2));
  }

  [Fact]
  public async Task Debit_WhenCalledConcurrentlyTotalExceedsBalance_BalanceIsPositive()
  {
    var account = CreateAccount();
    account.Credit(100);
    var count = 10;
    var debitAmount = 20;
    Task[] tasks = new Task[count];

    for (int i = 0; i < count; i++)
    {
      tasks[i] = Task.Run(() => account.Debit(debitAmount));
    }

    await Assert.ThrowsAsync<InsufficientFundsException>(async () => await Task.WhenAll(tasks));
    Assert.True(account.Balance >= 0);
  }

  [Fact]
  public async Task Debit_WhenCalledConcurrentlyTotalNotExceedsBalance_CorrectBalance()
  {
    var account = CreateAccount();
    account.Credit(100);
    var count = 4;
    var debitAmount = 20;
    Task[] tasks = new Task[count];

    for (int i = 0; i < count; i++)
    {
      tasks[i] = Task.Run(() => account.Debit(debitAmount));
    }

    await Task.WhenAll(tasks);

    Assert.Equal(20, account.Balance);
  }
}
