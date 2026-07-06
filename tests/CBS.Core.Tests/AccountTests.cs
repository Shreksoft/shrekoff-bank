using CBS.Core.Ledger.Domain;
using CBS.Core.Ledger.Domain.Exceptions;

namespace CBS.Core.Tests;

public class AccountTests
{
  private Account CreateAccount() => new(Guid.NewGuid(), Currency.Ruble);

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
  public void Debit_InsufficientBalance_ThrowsError()
  {
    var acc = CreateAccount();
    var exception = Assert.Throws<InsufficientFundsException>(() => acc.Debit(100));
  }

  [Fact]
  public void Debit_AccountBlocked_ThrowsError()
  {
    var acc = CreateAccount();
    acc.Block();
    var exception = Assert.Throws<AccountBlockedException>(() => acc.Debit(100));
  }

  [Fact]
  public void Debit_AmountIsNegative_ThrowsError()
  {
    var acc = CreateAccount();
    var amount = -1;
    var exception = Assert.Throws<AmountIsNegativeException>(() => acc.Debit(amount));

  }

  [Fact]
  public void Credit_AccountBlocked_ThrowsError()
  {
    var id = Guid.NewGuid();
    var acc = new Account(id, Currency.Ruble);

    acc.Block();

    var exception = Assert.Throws<AccountBlockedException>(() => acc.Credit(100));
  }

  [Fact]
  public void Credit_AmountIsNegative_ThrowsError()
  {
    var acc = CreateAccount();
    var amount = -1;
    var exception = Assert.Throws<AmountIsNegativeException>(() => acc.Credit(amount));

  }
}
