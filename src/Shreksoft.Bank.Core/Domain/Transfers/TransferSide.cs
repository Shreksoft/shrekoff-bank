using Shreksoft.Bank.Core.Domain.Shared.Currencies;

namespace Shreksoft.Bank.Core.Domain.Transfers;

public readonly record struct TransferSide(Guid AccountId, Guid ClientId)
{
    // for EF
    public TransferSide() : this(Guid.Empty, Guid.Empty)
    {
    }
}
