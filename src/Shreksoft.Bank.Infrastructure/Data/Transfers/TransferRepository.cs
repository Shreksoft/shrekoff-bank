using Shreksoft.Bank.Application.Accounts;
using Shreksoft.Bank.Core.Domain.Transfers;

namespace Shreksoft.Bank.Infrastructure.Data.Transfers;

public class TransferRepository(BankDbContext context) : ITransferRepository
{
    public void Add(Transfer transfer)
    {
        context.Transfers.Add(transfer);
    }
}
