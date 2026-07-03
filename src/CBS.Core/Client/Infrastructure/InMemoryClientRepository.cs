using CBS.Core.Client.Services;

namespace CBS.Core.Client.Infrastructure;

class InMemoryClientRepository : IClientRepository
{
  private readonly Dictionary<Guid, Domain.Client> _clients = [];

  public IReadOnlyCollection<Domain.Client> FindByEmail(string email)
  {
    return _clients.Values.Where(client => client.Info.Email == email).ToArray();
  }

  public Domain.Client? FindById(Guid clientId)
  {
    return _clients.GetValueOrDefault(clientId);
  }

  public IReadOnlyCollection<Domain.Client> FindByPhoneNumber(string phoneNumber)
  {
    return _clients.Values.Where(client => client.Info.PhoneNumber == phoneNumber).ToArray();
  }

  public void Save(Domain.Client client)
  {
    _clients[client.Id] = client;
  }
}