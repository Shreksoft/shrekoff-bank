using Shreksoft.Bank.Application.Accounts;
using Shreksoft.Bank.Core.Domain.Accounts;

namespace Shreksoft.Bank.Infrastructure.Data.Accounts;

public class AccountRepository(BankDbContext context) : IAccountRepository
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
