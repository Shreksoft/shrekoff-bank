namespace CBS.Core.Services.Ledger;

public class InMemoryAccountRepository : IAccountRepository
{
  private readonly Dictionary<Guid, Account> _accounts = [];
  
  public Account? Find(Guid accountId)
  {
    return _accounts.GetValueOrDefault(accountId);
  }

  public void Save(Account account)
  {
    Account? accountInDb = Find(account.Id);
    if (accountInDb != null)
    {
      _accounts.Add(account.Id, account);
    }
    else
    {
      throw new InvalidOperationException();
    }
  }
}