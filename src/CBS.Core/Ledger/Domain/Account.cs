namespace CBS.Core.Ledger.Domain;

public enum Currency { Ruble, Dollar, Euro }

public class Account(Guid clientId, Currency currency)
{
  public Guid Id { get; } = Guid.NewGuid();
  public Guid ClientId { get; } = clientId;
  public Currency Currency { get; } = currency;

  private decimal _balance;
  public decimal Balance => _balance;

  private bool _isBlocked;
  public bool IsBlocked => _isBlocked;

  public void Debit(decimal amount)
  {
    if (_balance - amount < 0)
    {
      throw new InvalidOperationException();
    }
    
    _balance -= amount;
  }

  public void Credit(decimal amount)
  {
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
}