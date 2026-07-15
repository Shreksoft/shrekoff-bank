using CBS.Core.Accounts.Domain.Exceptions;
using CBS.Core.Accounts.Exceptions;

namespace CBS.Core.Accounts.Domain;

public enum Currency { Ruble, Dollar, Euro }

public class Account(Guid clientId, Currency currency)
{
  public Guid Id { get; } = Guid.NewGuid();
  public Guid ClientId { get; } = clientId;
  public Currency Currency { get; } = currency;

  private decimal _balance;
  public decimal Balance => _balance;
  private readonly object _locker = new();
  private bool _isBlocked;
  public bool IsBlocked => _isBlocked;

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
      if (_isBlocked) throw new AccountBlockedException(Id);

      if (amount > _balance) throw new InsufficientFundsException(Id, Balance);

      _balance -= amount;
    }
    finally
    {
      Monitor.Exit(_locker);
    }
  }

  public void Credit(decimal amount)
  {
    if (amount < 0) throw new AmountIsNegativeException(Id, amount);

    if (_isBlocked) throw new AccountBlockedException(Id);

    _balance += amount;
  }

  public void Block()
  {
    _isBlocked = true;
  }

  public void Unblock()
  {
    _isBlocked = false;
  }

  public void EnsureSameCurrency(Account other)
  {
    if (Currency != other.Currency)
      throw new CurrencyMismatchException(Currency, other.Currency);
  }
}
