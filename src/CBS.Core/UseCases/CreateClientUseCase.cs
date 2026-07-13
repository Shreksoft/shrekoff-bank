using CBS.Core.Clients.Domain;
using CBS.Core.Clients.Services;

namespace CBS.Core.UseCases;

public class CreateClientUseCase
{
  private readonly ClientService _clientService;

  private const byte AdultAge = 18;

  internal CreateClientUseCase(ClientService clientService)
  {
    _clientService = clientService;
  }

  public Guid Execute(ClientInfo clientInfo)
  {
    if (clientInfo.BirthDate.Age < AdultAge)
    {
      throw new InvalidOperationException($"Age({clientInfo.BirthDate.Age}) is small for adult account (adult is {AdultAge})");
    }

    var id = _clientService.CreateClient(clientInfo);
    return id;
  }
}
