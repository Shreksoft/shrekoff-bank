using CBS.Core.Accounts.Domain;

namespace CBS.Core.Accounts.Services;

public interface IAccountRepository
{
  public Account? FindById(Guid accountId);
  public void Add(Account account);
  public void Update(Account account);
}
