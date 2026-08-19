using CBS.Core.Accounts.Domain.Exceptions;

namespace CBS.Core.Accounts.Domain;

public class Account(Guid id, Guid clientId, Money money, bool isBlocked)
{
  public Guid Id { get; } = id;
  public Guid ClientId { get; } = clientId;
  public Money Money { get; private set; } = money;
  public bool IsBlocked { get; private set; } = isBlocked;

  public Account(Guid clientId, Money money) : this(Guid.NewGuid(), clientId, money, false) { }

  // for EFCore ()
  private Account() : this(Guid.Empty, Guid.Empty, default, false) { }

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

  public void EnsureSameCurrency(Account other)
  {
    if (Money.Currency != other.Money.Currency)
      throw new CurrencyMismatchException(Money.Currency, other.Money.Currency);
  }
}
