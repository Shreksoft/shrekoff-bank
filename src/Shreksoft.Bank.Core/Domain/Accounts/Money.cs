using Shreksoft.Bank.Core.Domain.Shared.Currencies;

namespace Shreksoft.Bank.Core.Domain.Accounts;

public readonly record struct Money(Currency Currency, decimal Amount)
{
    // for EF
    public Money() : this(default, 0)
    {
    }
}
