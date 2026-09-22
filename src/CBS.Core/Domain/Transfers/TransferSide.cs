using CBS.Core.Accounts.Domain.Currencies;

namespace CBS.Core.Domain.Transfers;

public readonly record struct TransferSide(Guid AccountId, Guid ClientId, Currency Currency)
{
  public Guid AccountId { get; } = AccountId;
  public Guid ClientId { get; } = ClientId;
  public Currency Currency { get; } = Currency;

  // for EF
  public TransferSide() : this(Guid.Empty, Guid.Empty, default) {}
}
