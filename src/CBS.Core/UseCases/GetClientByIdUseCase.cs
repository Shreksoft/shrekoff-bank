using CBS.Core.Clients.Domain;
using CBS.Core.Clients.Services;

namespace CBS.Core.UseCases;

public class GetClientByIdUseCase(ClientService clientService)
{
  public Client? Execute(Guid id)
  {
    return clientService.GetClientById(id);
  }
}
