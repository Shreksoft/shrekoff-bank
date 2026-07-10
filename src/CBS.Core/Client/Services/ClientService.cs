using CBS.Core.Client.Domain;

namespace CBS.Core.Client.Services;

class ClientService(IClientRepository repository)
{
  public Guid CreateClient(ClientInfo clientInfo)
  {
    var client = new Domain.Client(clientInfo);
    repository.Save(client);

    return client.Id;
  }

  public bool ClientExists(Guid clientId)
  {
    var client = repository.FindById(clientId);
    return client != null;
  }
}
