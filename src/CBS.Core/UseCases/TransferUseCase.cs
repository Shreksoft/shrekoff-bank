using CBS.Core.Accounts.Services;

namespace CBS.Core.UseCases;

public class TransferUseCase
{
  private readonly AccountService _accountService;

  internal TransferUseCase(AccountService accountService) => _accountService = accountService;

  public Guid Execute(Guid senderAccountId, Guid recipientAccountId, decimal amount) => _accountService.Transfer(senderAccountId, recipientAccountId, amount);
}
