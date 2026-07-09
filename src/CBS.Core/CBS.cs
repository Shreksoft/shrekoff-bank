using CBS.Core.Client.Infrastructure;
using CBS.Core.Client.Services;
using CBS.Core.Ledger.Infrastructure;
using CBS.Core.Ledger.Services;
using CBS.Core.UseCases;

namespace CBS.Core;

public class CBS
{
  private readonly ILedger _ledger = new LedgerService(new InMemoryAccountRepository());
  private readonly ClientService _clientService = new(new InMemoryClientRepository());

  public CreateAccountForClientUseCase CreateAccountForClientUseCase() => new(_clientService, _ledger);

  public CreateClientUseCase CreateClientUseCase() => new(_clientService);
}

