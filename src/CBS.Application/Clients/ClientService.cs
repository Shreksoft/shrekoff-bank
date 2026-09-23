using CBS.Core.Domain.Clients;
using ObjectNotFoundException = CBS.Application.Shared.ObjectNotFoundException;

namespace CBS.Application.Clients;

public class ClientService(IUnitOfWork unitOfWork, IClientRepository repository)
{
  public Client CreateClient(ClientInfo clientInfo)
  {
    var client = new Client(clientInfo);
    repository.Add(client);
    unitOfWork.SaveChanges();

    return client;
  }

  public Client GetByIdOrThrow(Guid id)
  {
    return repository.FindById(id) ?? throw new ObjectNotFoundException(id);
  }
}
