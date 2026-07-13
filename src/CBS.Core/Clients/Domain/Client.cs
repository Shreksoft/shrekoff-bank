namespace CBS.Core.Clients.Domain;

class Client(ClientInfo clientInfo)
{
  public ClientInfo Info { get; private set; } = clientInfo;
  public Guid Id { get; } = Guid.NewGuid();
  public DateTime CreatedDate { get; } = DateTime.Now;

  public void ChangeEmail(string newEmail)
  {
    Info = Info with { Email = newEmail };
  }
}
