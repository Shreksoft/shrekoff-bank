using CBS.Core.Accounts.Domain;
using CBS.Core.Accounts.Services;
using CBS.Core.Clients.Services;

namespace CBS.Core.UseCases;

public class CreateAccountUseCase(ClientService clientService, AccountService accountService)
{
  public Guid Execute(Guid clientId, Currency currency)
  {
    //ensure that client exists
    clientService.GetById(clientId);

    var account = accountService.CreateAccount(clientId, currency);
    return account.Id;
  }
}
