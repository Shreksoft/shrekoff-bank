using CBS.Core.Client.Domain;

namespace CBS.Core.Client.Services;

interface IClientRepository
{
  public void Save(Domain.Client client);
  public Domain.Client? FindById(Guid clientId);
  public IReadOnlyCollection<Domain.Client> FindByPhoneNumber(string phoneNumber);
  public IReadOnlyCollection<Domain.Client> FindByEmail(string email);
}