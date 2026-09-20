using CBS.Core.Accounts.Domain;

namespace CBS.Application.Accounts;

public interface IAccountRepository
{
  public Account? FindById(Guid accountId);
  public void Add(Account account);
}
