using CBS.Application.Clients;
using CBS.Core.Domain.Clients;

namespace CBS.Infrastructure.Data.Clients;

public class ClientRepository(CbsContext context) : IClientRepository
{
  public Client? FindById(Guid clientId)
  {
    return context.Clients.Find(clientId);
  }

  public void Add(Client client)
  {
    context.Clients.Add(client);
  }
}
