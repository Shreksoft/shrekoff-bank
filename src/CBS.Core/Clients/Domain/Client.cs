namespace CBS.Core.Clients.Domain;

public class Client(Guid id, ClientInfo clientInfo, DateTime createdDate)
{

  public Client(ClientInfo clientInfo) : this(Guid.NewGuid(), clientInfo, DateTime.UtcNow) { }
  public ClientInfo Info { get; private set; } = clientInfo;
  public Guid Id { get; } = id;
  public DateTime CreatedDate { get; } = createdDate;

  public void ChangeEmail(string newEmail)
  {
    Info = Info with { Email = newEmail };
  }
}
