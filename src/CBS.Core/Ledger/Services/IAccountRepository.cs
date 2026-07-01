using CBS.Core.Ledger.Domain;

namespace CBS.Core.Ledger.Services;

public interface IAccountRepository
{
  public Account? FindById(Guid accountId);
  public IReadOnlyCollection<Account> FindByClientId(Guid clientId);
  public void Save(Account account);
}