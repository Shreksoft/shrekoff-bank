using CBS.Core.Accounts.Domain.Exceptions;

namespace CBS.Core.Accounts.Domain;

public class Account(Guid clientId, Money money)
{
  public Guid Id { get; } = Guid.NewGuid();
  public Guid ClientId { get; } = clientId;
  public Money Money { get; private set; } = money;
  private readonly object _locker = new();
  public bool IsBlocked { get; private set; }

  public void Debit(decimal amount)
  {
    if (amount <= 0) throw new AmountIsNegativeException(Id, amount);

    /**
    Monitor - механизм синхронизации доступа при многопотоке, один из потоков, который попадает в этот метод занимает объект _locker это сигнал другим потокам встать в ожидание пока _locker не освободится
    можно использовать lock(_locker){} - это сс
    **/
    Monitor.Enter(_locker);
    try
    {
      if (IsBlocked) throw new AccountBlockedException(Id);

      var balance = Money.Amount;
      if (amount > balance) throw new InsufficientFundsException(Id, balance);

      Money = Money with { Amount = balance - amount };
    }
    finally
    {
      Monitor.Exit(_locker);
    }
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
