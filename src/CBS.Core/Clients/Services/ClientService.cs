using CBS.Core.Clients.Domain;
using CBS.Core.Exceptions;

namespace CBS.Core.Clients.Services;

public class ClientService(IUnitOfWork unitOfWork, IClientRepository repository)
{
  public Guid CreateClient(ClientInfo clientInfo)
  {
    var client = new Client(clientInfo);
    repository.Add(client);
    unitOfWork.SaveChanges();

    return client.Id;
  }

  public Client GetById(Guid id)
  {
    return repository.FindById(id) ?? throw new ObjectNotFoundException(id);
  }
}
