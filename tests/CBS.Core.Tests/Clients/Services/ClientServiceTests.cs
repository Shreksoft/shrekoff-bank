using CBS.Application;
using CBS.Application.Clients;
using CBS.Core.Domain.Clients;
using Moq;

namespace CBS.Core.Tests.Clients.Services;

public class ClientServiceTests
{
  private readonly ClientService _clientService;
  private readonly Mock<IClientRepository> _clientRepository = new();
  private readonly Mock<IUnitOfWork> _unitOfWork = new();

  public ClientServiceTests()
  {
    _clientService = new ClientService(_unitOfWork.Object, _clientRepository.Object);
  }

  [Fact]
  public void CreateClient_CorrectClientData_ClientAddedInBase()
  {
    var ci = new ClientInfo(
      new FullName("1", "2", "3"),
      new BirthDate(new DateOnly(1990, 01, 01)),
      null,
      null
    );

    var client = _clientService.CreateClient(ci);

    _clientRepository.Verify(r => r.Add(client), Times.Once());
    _unitOfWork.Verify(u => u.SaveChanges(), Times.Once());

    _clientRepository.Setup(r => r.FindById(client.Id)).Returns(client);
    var clientFromService = _clientService.GetByIdOrThrow(client.Id);
    Assert.Equal(clientFromService.Id, client.Id);
    Assert.Equal(ci, clientFromService.Info);
  }
}
