using Shreksoft.Bank.Core.Domain.Shared.Currencies;

namespace Shreksoft.Bank.Core.Domain.Transfers;

public readonly record struct TransferSide(Guid AccountId, Guid ClientId, Currency Currency)
{
    public Guid AccountId { get; } = AccountId;
    public Guid ClientId { get; } = ClientId;
    public Currency Currency { get; } = Currency;

    // for EF
    public TransferSide() : this(Guid.Empty, Guid.Empty, default)
    {
    }
}
