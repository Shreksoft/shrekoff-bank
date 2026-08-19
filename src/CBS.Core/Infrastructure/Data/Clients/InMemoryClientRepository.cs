using CBS.Core.Clients.Domain;
using CBS.Core.Clients.Services;

namespace CBS.Core.Infrastructure.Data.Clients;

public class InMemoryClientRepository(ITable<Client> table, IChangeTracker changeTracker) : IClientRepository
{
  public void Add(Client client)
  {
    changeTracker.AddChange(() => table.Insert(client.Id, Clone(client)));
  }

  public Client? FindById(Guid clientId)
  {
    var client = table.GetStorage().GetValueOrDefault(clientId);
    return client == null ? null : Clone(client);
  }

  private static Client Clone(Client client)
  {
    return new Client(client.Id, client.Info, client.CreatedDate);
  }
}
