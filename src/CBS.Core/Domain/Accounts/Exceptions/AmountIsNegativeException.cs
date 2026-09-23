namespace CBS.Core.Domain.Accounts.Exceptions;

public sealed class AmountIsNegativeException(Guid accountId, decimal amount) : Exception($"Amount must be positive")
{
  public Guid AccountId { get; } = accountId;
  public decimal Amount { get; } = amount;
}
