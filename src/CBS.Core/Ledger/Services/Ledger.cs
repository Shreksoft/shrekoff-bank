using CBS.Core.Ledger.Domain;

namespace CBS.Core.Ledger.Services;

public class Ledger(IAccountRepository repository) : ILedger
{
  public Account CreateAccount(Guid clientId, Currency currency)
  {
    var account = new Account(clientId, currency);
    repository.Save(account);
    return account;
  }

  public void OpenAccount(Guid accountId)
  {
    var account = GetAccountOrThrow(accountId);
    account.Unblock();
  }

  public void CloseAccount(Guid accountId)
  {
    var account = GetAccountOrThrow(accountId);
    account.Block();
  }

  public void Transaction(Guid accountFromId, Guid accountToId, decimal amount)
  {
    var from = GetAccountOrThrow(accountFromId);
    var to = GetAccountOrThrow(accountToId);

    if (from.Currency != to.Currency) 
      throw new InvalidOperationException("Accounts doesn't have the same currency");
    
    from.Debit(amount);
    to.Credit(amount);
  }

  private Account GetAccountOrThrow(Guid accountId)
  {
    return repository.FindById(accountId)
                  ?? throw new InvalidOperationException($"Account {accountId} not found");
  }
}