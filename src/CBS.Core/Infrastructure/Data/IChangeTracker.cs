namespace CBS.Core.Infrastructure.Data;

public interface IChangeTracker
{
  public void AddChange(Action action);
}
