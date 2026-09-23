using CBS.Core.Domain.Shared.Currencies;

namespace CBS.Core.Domain.Accounts;

public readonly record struct Money(Currency Currency, decimal Amount)
{
  // for EF
  public Money() : this(default, 0)
  {
  }
}
