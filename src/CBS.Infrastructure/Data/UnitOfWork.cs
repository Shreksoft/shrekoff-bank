using CBS.Core;

namespace CBS.Infrastructure.Data;

public class UnitOfWork(CbsContext context) : IUnitOfWork
{
  public int SaveChanges()
  {
    return context.SaveChanges();
  }
}
