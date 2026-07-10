using CBS.Core.Accounts.Domain;

namespace CBS.Core.Accounts.Services;

public interface IAccountRepository
{
  public Account? FindById(Guid accountId);
  public IReadOnlyCollection<Account> FindByClientId(Guid clientId);
  public void Save(Account account);
}
