// TODO: implement UoW through dictionary. Rewrite dictionary in InMemory* to stack logic. UoW will be contained dictionary. Short logic -> note/put in stack entity/data [InMemory*] after in UoW need fully clean stack.

namespace CBS.Core.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork, IChangeTracker
{
  private readonly Queue<Action> _tasks = [];

  public void AddChange(Action action)
  {
    _tasks.Enqueue(action);
  }

  public int SaveChanges()
  {
    var workCount = _tasks.Count;
    while (_tasks.Count > 0)
    {
      _tasks.Dequeue().Invoke();
    }

    return workCount;
  }
}
