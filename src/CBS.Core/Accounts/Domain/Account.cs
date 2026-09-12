using CBS.Core.Accounts.Domain.Currencies;
using CBS.Core.Accounts.Domain.Exceptions;

namespace CBS.Core.Accounts.Domain;

public class Account(Guid id, Guid clientId, Money money, bool isBlocked)
{
  public Account(Guid clientId, Money money) : this(Guid.NewGuid(), clientId, money, false)
  {
    var isValidCcy = Currency.IsValid(money.Currency);
    if (!isValidCcy)
      throw new ArgumentException("Invalid currency");
  }

  // for EFCore ()
  private Account() : this(Guid.Empty, Guid.Empty, default, false)
  {
  }

  public Guid Id { get; init; } = id;
  public Guid ClientId { get; private set; } = clientId;
  public Money Money { get; private set; } = money;
  public bool IsBlocked { get; private set; } = isBlocked;

  public void Debit(decimal amount)
  {
    if (amount <= 0) throw new AmountIsNegativeException(Id, amount);
    if (IsBlocked) throw new AccountBlockedException(Id);

    var balance = Money.Amount;
    if (amount > balance) throw new InsufficientFundsException(Id, balance);

    Money = Money with { Amount = balance - amount };
  }

  public void Credit(decimal amount)
  {
    if (amount <= 0) throw new AmountIsNegativeException(Id, amount);
    if (IsBlocked) throw new AccountBlockedException(Id);

    Money = Money with { Amount = Money.Amount + amount };
  }

  public void Block()
  {
    IsBlocked = true;
  }

  public void Unblock()
  {
    IsBlocked = false;
  }
}
