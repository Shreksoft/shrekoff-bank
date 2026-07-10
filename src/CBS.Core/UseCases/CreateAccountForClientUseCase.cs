using CBS.Core.Client.Services;
using CBS.Core.Accounts.Domain;
using CBS.Core.Accounts.Services;

namespace CBS.Core.UseCases;

public class CreateAccountForClientUseCase
{
  private readonly AccountService _accountService;
  private readonly ClientService _clientService;

  internal CreateAccountForClientUseCase(ClientService clientService, AccountService accountService)
  {
    _accountService = accountService;
    _clientService = clientService;
  }

  public Guid Execute(Guid clientId, Currency currency)
  {
    if (!_clientService.ClientExists(clientId))
      throw new ArgumentException("client doesn't exist");

    var account = _accountService.CreateAccount(clientId, currency);
    return account.Id;
  }
}
