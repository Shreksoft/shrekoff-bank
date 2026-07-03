using CBS.Core.Ledger.Domain;
using CBS.Core.Ledger.Services;

namespace CBS.Core.Ledger.Infrastructure;

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
    _accounts[account.Id] = account;
  }
}