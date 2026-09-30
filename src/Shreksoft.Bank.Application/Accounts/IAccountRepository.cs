using Shreksoft.Bank.Core.Domain.Accounts;

namespace Shreksoft.Bank.Application.Accounts;

public interface IAccountRepository
{
    public Account? FindById(Guid accountId);
    public void Add(Account account);
}
