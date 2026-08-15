namespace CBS.Core.Infrastructure.Data;

public interface ITable<T>
{
  public void Insert(Guid key, T value);
  public void Update(Guid key, T value);
  public IReadOnlyDictionary<Guid, T> GetStorage();
}
