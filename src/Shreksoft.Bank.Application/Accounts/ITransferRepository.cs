using Shreksoft.Bank.Core.Domain.Transfers;

namespace Shreksoft.Bank.Application.Accounts;

public interface ITransferRepository
{
    public void Add(Transfer transfer);
}
