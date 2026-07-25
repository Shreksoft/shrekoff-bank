using CBS.Core.Clients.Domain;
using CBS.Core.Clients.Services;

namespace CBS.Core.UseCases;

public class CreateClientUseCase(ClientService clientService)
{
  private const byte AdultAge = 18;

  public Guid Execute(ClientInfo clientInfo)
  {
    if (clientInfo.BirthDate.Age < AdultAge)
      throw new InvalidOperationException(
        $"Age({clientInfo.BirthDate.Age}) is small for adult account (adult is {AdultAge})");

    var id = clientService.CreateClient(clientInfo);
    return id;
  }
}
