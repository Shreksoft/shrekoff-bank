using CBS.Core.Clients.Domain;
using CBS.Core.Clients.Services;

namespace CBS.Core.UseCases;

public class GetClientByIdUseCase
{
  private readonly ClientService _clientService;

  internal GetClientByIdUseCase(ClientService clientService) => _clientService = clientService;

  public Client? Execute(Guid id) => _clientService.GetClientById(id);
}
