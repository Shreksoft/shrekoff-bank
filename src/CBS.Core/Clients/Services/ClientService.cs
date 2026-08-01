using CBS.Core.Clients.Domain;
using CBS.Core.Exceptions;

namespace CBS.Core.Clients.Services;

public class ClientService(IClientRepository repository)
{
  public Guid CreateClient(ClientInfo clientInfo)
  {
    var client = new Client(clientInfo);
    repository.Save(client);

    return client.Id;
  }

  public Client GetById(Guid id)
  {
    return repository.FindById(id) ?? throw new ObjectNotFoundException(id);
  }
}
