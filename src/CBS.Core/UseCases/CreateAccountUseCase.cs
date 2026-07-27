using CBS.Core.Accounts.Domain;
using CBS.Core.Accounts.Services;
using CBS.Core.Clients.Services;

namespace CBS.Core.UseCases;

public class CreateAccountUseCase(ClientService clientService, AccountService accountService)
{
  public Guid Execute(Guid clientId, Currency currency)
  {
    if (!clientService.ClientExists(clientId))
      throw new ArgumentException("Client doesn't exist", nameof(clientId));

    var account = accountService.CreateAccount(clientId, currency);
    return account.Id;
  }
}
