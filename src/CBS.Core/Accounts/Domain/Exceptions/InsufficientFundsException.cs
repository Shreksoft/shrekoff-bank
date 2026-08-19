namespace CBS.Core.Accounts.Domain.Exceptions;

public sealed class InsufficientFundsException(Guid accountId, decimal balance) : Exception($"Account {accountId}: insufficient funds. Balance: {balance}")
{
  public Guid AccountId { get; } = accountId;
  public decimal Balance { get; } = balance;
}
