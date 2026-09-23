using CBS.Core.Accounts.Domain;
using CBS.Core.Domain.Accounts;

namespace CBS.Application.Accounts;

public interface IAccountRepository
{
  public Account? FindById(Guid accountId);
  public void Add(Account account);
}
