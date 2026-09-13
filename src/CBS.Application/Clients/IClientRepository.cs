using CBS.Core.Clients.Domain;

namespace CBS.Application.Clients;

public interface IClientRepository
{
  public void Add(Client client);
  public Client? FindById(Guid clientId);
}
