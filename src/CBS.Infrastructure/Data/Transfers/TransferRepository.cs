using CBS.Application.Accounts;
using CBS.Core.Domain.Transfers;

namespace CBS.Infrastructure.Data.Transfers;

public class TransferRepository(CbsContext context) : ITransferRepository
{
  public void Add(Transfer transfer)
  {
    context.Transfers.Add(transfer);
  }
}
