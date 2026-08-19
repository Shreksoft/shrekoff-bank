using CBS.Core.Accounts.Services;

namespace CBS.Core.UseCases;

public class TransferUseCase(AccountService accountService)
{
  public Guid Execute(Guid senderAccountId, Guid recipientAccountId, decimal amount)
  {
    return accountService.Transfer(senderAccountId, recipientAccountId, amount);
  }
}
