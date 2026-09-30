namespace Shreksoft.Bank.Application.Shared;

public class ObjectNotFoundException(Guid id) : Exception($"{id} {Msg}")
{
    private const string Msg = "Object with this ID isn't found";
}
