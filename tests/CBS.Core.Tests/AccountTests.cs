using CBS.Core.Accounts.Domain;
using CBS.Core.Accounts.Domain.Exceptions;
using CBS.Core.Accounts.Exceptions;

namespace CBS.Core.Tests;

public class AccountTests
{
  private Account CreateAccount(Currency currency = Currency.Ruble) => new(Guid.NewGuid(), currency);

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
    var acc = new Account(id, Currency.Ruble);

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
  public void EnsureSameCurrency_CurrenciesTheDiffrent_Throws()
  {
    var acc1 = CreateAccount();
    var acc2 = CreateAccount(Currency.Dollar);

    Assert.Throws<CurrencyMismatchException>(() => acc1.EnsureSameCurrency(acc2));
  }
}
