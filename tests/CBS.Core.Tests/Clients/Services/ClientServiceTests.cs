using CBS.Core.Clients.Domain;
using CBS.Core.Clients.Services;
using CBS.Core.Infrastructure.Data;
using CBS.Core.Infrastructure.Data.Clients;

namespace CBS.Core.Tests.Clients.Services;

public class ClientServiceTests
{
  [Fact]
  public void CreateClient_CorrectClientData_ClientExistsInBase()
  {
    var table = new Table<Client>();
    var unitOfWork = new UnitOfWork();
    var repo = new InMemoryClientRepository(table, unitOfWork);
    var clientService = new ClientService(unitOfWork, repo);
    var ci = new ClientInfo(
      new FullName("1", "2", "3"),
      new BirthDate(new DateOnly(1990, 01, 01)),
      null,
      null
    );

    var ex = Record.Exception(() => clientService.CreateClient(ci));
    Assert.Null(ex);
  }
}
