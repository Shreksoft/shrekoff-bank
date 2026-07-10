using CBS.Core.Clients.Domain;
using CBS.Core.Clients.Services;

namespace CBS.Core.UseCases;

public class CreateClientUseCase
{
  private readonly ClientService _clientService;

  internal CreateClientUseCase(ClientService clientService)
  {
    _clientService = clientService;
  }

  public Guid Execute(ClientInfo clientInfo)
  {
    var id = _clientService.CreateClient(clientInfo);
    return id;
  }
}
