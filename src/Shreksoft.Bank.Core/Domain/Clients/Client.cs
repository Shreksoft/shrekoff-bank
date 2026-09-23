namespace Shreksoft.Bank.Core.Domain.Clients;

public class Client(Guid id, ClientInfo clientInfo, DateTime createdDate)
{
    public Client(ClientInfo clientInfo) : this(Guid.NewGuid(), clientInfo, DateTime.UtcNow)
    {
    }

    // for EFCore
    private Client() : this(Guid.Empty, default, default)
    {
    }

    public ClientInfo Info { get; private set; } = clientInfo;
    public Guid Id { get; private set; } = id;
    public DateTime CreatedDate { get; private set; } = createdDate;
}
