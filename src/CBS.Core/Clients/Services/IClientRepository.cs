using CBS.Core.Clients.Domain;

namespace CBS.Core.Clients.Services;

public interface IClientRepository
{
  public void Add(Client client);
  public Client? FindById(Guid clientId);
}
