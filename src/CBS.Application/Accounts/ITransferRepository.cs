using CBS.Core.Domain.Transfers;

namespace CBS.Application.Accounts;

public interface ITransferRepository
{
  public void Add(Transfer transfer);
}
