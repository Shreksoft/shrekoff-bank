using CBS.Core.Domain.Clients;

namespace CBS.Application.Clients;

public interface IClientRepository
{
  public void Add(Client client);
  public Client? FindById(Guid clientId);
}
