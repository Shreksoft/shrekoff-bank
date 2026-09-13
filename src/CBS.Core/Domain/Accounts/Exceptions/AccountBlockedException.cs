namespace CBS.Core.Accounts.Domain.Exceptions;

public sealed class AccountBlockedException(Guid accountId) : Exception($"Account {accountId} is blocked")
{
  public Guid AccountId { get; } = accountId;
}
