using CBS.Core.Accounts.Domain.Currencies;
using CBS.Core.Accounts.Domain.Exceptions;

namespace CBS.Core.Accounts.Domain;

public class Account(Guid id, Guid clientId, Money money, bool isBlocked)
{
  public Account(Guid clientId, Money money) : this(Guid.NewGuid(), clientId, money, false)
  {
    if (!Currency.IsValid(money.Currency))
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

    var newAmount = Math.Round(balance - amount, Money.Currency.Scale);
    Money = Money with { Amount = newAmount };
  }

  public void Credit(decimal amount)
  {
    if (amount <= 0) throw new AmountIsNegativeException(Id, amount);
    if (IsBlocked) throw new AccountBlockedException(Id);

    var newAmount = Math.Round(Money.Amount + amount, Money.Currency.Scale);
    Money = Money with { Amount = newAmount };
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
