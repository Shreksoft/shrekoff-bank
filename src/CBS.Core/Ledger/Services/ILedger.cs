using CBS.Core.Ledger.Domain;

namespace CBS.Core.Ledger.Services;

public interface ILedger
{
  public Account CreateAccount(Guid clientId, Currency currency);
  public void OpenAccount(Guid accountId);
  public void CloseAccount(Guid accountId);
  public void Transaction(Guid accountFrom, Guid accountTo, decimal amount);
}