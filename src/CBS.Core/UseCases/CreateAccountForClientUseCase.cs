using CBS.Core.Client.Services;
using CBS.Core.Ledger.Domain;
using CBS.Core.Ledger.Services;

namespace CBS.Core.UseCases;

public class CreateAccountForClientUseCase
{
  private readonly ILedger _ledger;
  private readonly ClientService _clientService;

  internal CreateAccountForClientUseCase(ClientService clientService, ILedger ledger)
  {
    _ledger = ledger;
    _clientService = clientService;
  }

  public Guid Execute(Guid clientId, Currency currency)
  {
    if (!_clientService.ClientExists(clientId))
      throw new ArgumentException("client doesn't exist");

    var account = _ledger.CreateAccount(clientId, currency);
    return account.Id;
  }
}
