using CBS.Core.Accounts.Domain;
using CBS.Core.Accounts.Domain.Currencies;
using CBS.Core.Accounts.Services;
using CBS.Core.Clients.Services;

namespace CBS.Core.UseCases;

public class CreateAccountUseCase(ClientService clientService, AccountService accountService)
{
  public Guid Execute(Guid clientId, CurrencyCode currencyCode)
  {
    //ensure that client exists
    clientService.GetById(clientId);

    var money = new Money(new Currency(currencyCode), 0);
    var account = accountService.CreateAccount(clientId, money);
    return account.Id;
  }
}
