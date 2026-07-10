using CBS.Core.Client.Domain;
using CBS.Core.Client.Services;

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
