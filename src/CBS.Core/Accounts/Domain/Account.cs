using CBS.Core.Accounts.Domain.Exceptions;

namespace CBS.Core.Accounts.Domain;

public enum Currency { Ruble, Dollar, Euro }

public class Account(Guid clientId, Currency currency)
{
  public Guid Id { get; } = Guid.NewGuid();
  public Guid ClientId { get; } = clientId;
  public Currency Currency { get; } = currency;

  public decimal Balance { get; private set; }

  private readonly object _locker = new();
  public bool IsBlocked { get; private set; }

  public void Debit(decimal amount)
  {
    if (amount < 0) throw new AmountIsNegativeException(Id, amount);

    /**
    Monitor - механизм синхронизации доступа при многопотоке, один из потоков, который попадает в этот метод занимает объект _locker это сигнал другим потокам встать в ожидание пока _locker не освободится
    можно использовать lock(_locker){} - это сс
    **/
    Monitor.Enter(_locker);
    try
    {
      if (IsBlocked) throw new AccountBlockedException(Id);

      if (amount > Balance) throw new InsufficientFundsException(Id, Balance);

      Balance -= amount;
    }
    finally
    {
      Monitor.Exit(_locker);
    }
  }

  public void Credit(decimal amount)
  {
    if (amount < 0) throw new AmountIsNegativeException(Id, amount);

    if (IsBlocked) throw new AccountBlockedException(Id);

    Balance += amount;
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
    if (Currency != other.Currency)
      throw new CurrencyMismatchException(Currency, other.Currency);
  }
}
