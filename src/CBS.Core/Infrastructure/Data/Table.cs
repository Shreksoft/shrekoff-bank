namespace CBS.Core.Infrastructure.Data;

public class Table<T> : ITable<T>
{
  private readonly Dictionary<Guid, T> _storage = [];

  public IReadOnlyDictionary<Guid, T> GetStorage()
  {
    return _storage.AsReadOnly();
  }

  public void Insert(Guid key, T value)
  {
    _storage.Add(key, value);
  }

  public void Update(Guid key, T value)
  {
    _storage[key] = value;
  }
}
