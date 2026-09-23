using CBS.Application.Accounts;
using CBS.Core.Accounts.Domain;
using CBS.Core.Domain.Accounts;

namespace CBS.Infrastructure.Data.Accounts;

public class AccountRepository(CbsContext context) : IAccountRepository
{
  public Account? FindById(Guid accountId)
  {
    return context.Accounts.Find(accountId);
  }

  public void Add(Account account)
  {
    context.Accounts.Add(account);
  }

  public void Update(Account account)
  {
    context.Accounts.Update(account);
  }
}
