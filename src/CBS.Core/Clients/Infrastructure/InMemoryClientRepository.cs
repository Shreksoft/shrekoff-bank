using CBS.Core.Clients.Domain;
using CBS.Core.Clients.Services;

namespace CBS.Core.Clients.Infrastructure;

class InMemoryClientRepository : IClientRepository
{
  private readonly Dictionary<Guid, Client> _clients = [];

  public IReadOnlyCollection<Client> FindByEmail(string email)
  {
    return _clients.Values.Where(client => client.Info.Email == email).ToArray();
  }

  public Client? FindById(Guid clientId)
  {
    return _clients.GetValueOrDefault(clientId);
  }

  public IReadOnlyCollection<Client> FindByPhoneNumber(string phoneNumber)
  {
    return _clients.Values.Where(client => client.Info.PhoneNumber == phoneNumber).ToArray();
  }

  public void Save(Client client)
  {
    _clients[client.Id] = client;
  }
}
