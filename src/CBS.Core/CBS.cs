using CBS.Core.Clients.Infrastructure;
using CBS.Core.Clients.Services;
using CBS.Core.Accounts.Infrastructure;
using CBS.Core.Accounts.Services;
using CBS.Core.UseCases;

namespace CBS.Core;

public class CBS
{
  private readonly AccountService _accountService = new(new InMemoryAccountRepository());
  private readonly ClientService _clientService = new(new InMemoryClientRepository());

  public CreateAccountForClientUseCase CreateAccountForClientUseCase() => new(_clientService, _accountService);

  public CreateClientUseCase CreateClientUseCase() => new(_clientService);

  public TransferUseCase TransferUseCase() => new(_accountService);
}

