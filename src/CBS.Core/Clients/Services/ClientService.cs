using CBS.Core.Clients.Domain;

namespace CBS.Core.Clients.Services;

public class ClientService(IClientRepository repository)
{
  public Guid CreateClient(ClientInfo clientInfo)
  {
    var client = new Client(clientInfo);
    repository.Save(client);

    return client.Id;
  }

  public bool ClientExists(Guid clientId)
  {
    var client = repository.FindById(clientId);
    return client != null;
  }

  public Client? GetClientById(Guid id)
  {
    return repository.FindById(id);
  }
}
