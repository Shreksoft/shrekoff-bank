using CBS.Core.Accounts.Domain.Currencies;

namespace CBS.Core.Accounts.Domain;

public readonly record struct Money(Currency Currency, decimal Amount)
{
  // for EF
  public Money() : this(default, 0)
  {
  }
}
