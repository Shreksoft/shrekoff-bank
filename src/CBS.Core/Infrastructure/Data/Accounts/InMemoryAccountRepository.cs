using CBS.Core.Accounts.Domain;
using CBS.Core.Accounts.Services;

namespace CBS.Core.Infrastructure.Data.Accounts;

public class InMemoryAccountRepository(ITable<Account> table, IChangeTracker changeTracker) : IAccountRepository
{
  public Account? FindById(Guid accountId)
  {
    var account = table.GetStorage().GetValueOrDefault(accountId);
    return account == null ? null : Clone(account);
  }

  public void Add(Account account)
  {
    changeTracker.AddChange(() => table.Insert(account.Id, Clone(account)));
  }

  public void Update(Account account)
  {
    changeTracker.AddChange(() => table.Update(account.Id, Clone(account)));
  }

  private static Account Clone(Account account)
  {
    var clone = new Account(account.Id, account.ClientId, account.Money, account.IsBlocked);
    return clone;
  }
}
