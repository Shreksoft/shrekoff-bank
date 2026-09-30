using Shreksoft.Bank.Application;

namespace Shreksoft.Bank.Infrastructure.Data;

public class UnitOfWork(BankDbContext context) : IUnitOfWork
{
    public int SaveChanges()
    {
        return context.SaveChanges();
    }
}
