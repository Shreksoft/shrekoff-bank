using CBS.Core.Client.Domain;

namespace CBS.Core.Client.Services;

class ClientService(IClientRepository repository)
{
  private const byte MinAge = 18;
  private const ushort MinYear = 1900;

  public Domain.Client CreateClient(ClientInfo info)
  {
    if (info.BirthDate.Year < MinYear)
      throw new ArgumentException("Too old client");

    if (DateTime.Now.Year - info.BirthDate.Year < MinAge)
      throw new ArgumentException("Too young client");

    var client = new Domain.Client(info);
    repository.Save(client);

    return client;
  }
}