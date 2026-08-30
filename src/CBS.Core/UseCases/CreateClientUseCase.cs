using CBS.Core.Clients.Domain;
using CBS.Core.Clients.Services;

namespace CBS.Core.UseCases;

public class CreateClientUseCase(ClientService clientService)
{
  private const byte MinAge = 18;

  public Guid Execute(ClientInfo clientInfo)
  {
    if (clientInfo.BirthDate.Age < MinAge)
      throw new ArgumentException(
        $"Age({clientInfo.BirthDate.Age}) is small for creating account (account is allowed for {MinAge} age)");

    return clientService.CreateClient(clientInfo).Id;
  }
}
