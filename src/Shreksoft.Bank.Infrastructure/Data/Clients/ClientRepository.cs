using Shreksoft.Bank.Application.Clients;
using Shreksoft.Bank.Core.Domain.Clients;

namespace Shreksoft.Bank.Infrastructure.Data.Clients;

public class ClientRepository(BankDbContext context) : IClientRepository
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
