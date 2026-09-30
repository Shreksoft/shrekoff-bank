using Shreksoft.Bank.Core.Domain.Clients;

namespace Shreksoft.Bank.Application.Clients;

public interface IClientRepository
{
    public void Add(Client client);
    public Client? FindById(Guid clientId);
}
