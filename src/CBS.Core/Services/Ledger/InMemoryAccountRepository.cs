namespace CBS.Core.Services.Ledger;

public class InMemoryAccountRepository : IAccountRepository
{
  private readonly Dictionary<Guid, Account> _accounts = [];
  
  public Account? FindById(Guid accountId)
  {
    return _accounts.GetValueOrDefault(accountId);
  }

  public IReadOnlyCollection<Account> FindByClientId(Guid clientId)
  {
    return _accounts.Values.Where(acc => acc.ClientId == clientId).ToArray();
  }

  public void Save(Account account)
  {
    var memAcc = FindById(account.Id);
    if (memAcc != null)
    {
      throw new InvalidOperationException();
    }

    _accounts[account.Id] = account;
  }
}