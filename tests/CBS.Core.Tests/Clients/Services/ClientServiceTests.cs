using CBS.Core.Clients.Domain;
using CBS.Core.Clients.Infrastructure;
using CBS.Core.Clients.Services;

namespace CBS.Core.Tests.Clients.Services;

public class ClientServiceTests
{
  [Fact]
  public void CreateClient_CorrectClientData_ClientExistsInBase()
  {
    var clientService = new ClientService(new InMemoryClientRepository());
    var ci = new ClientInfo(
      new FullName("1", "2", "3"),
      new BirthDate(new DateOnly(1990, 01, 01)),
      null,
      null
    );
    var guid = clientService.CreateClient(ci);

    Assert.True(clientService.ClientExists(guid));
    Assert.NotNull(clientService.GetClientById(guid));
  }
}
