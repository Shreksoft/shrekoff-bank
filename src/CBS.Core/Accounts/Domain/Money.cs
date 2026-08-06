namespace CBS.Core.Accounts.Domain;

public readonly record struct Money(Currency Currency, decimal Amount);
