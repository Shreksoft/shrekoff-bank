using CBS.Core.Clients.Domain;

namespace CBS.Core.Clients.Services;

interface IClientRepository
{
  public void Save(Client client);
  public Client? FindById(Guid clientId);
  public IReadOnlyCollection<Client> FindByPhoneNumber(string phoneNumber);
  public IReadOnlyCollection<Client> FindByEmail(string email);
}
