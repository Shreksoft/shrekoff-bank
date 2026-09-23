using Shreksoft.Bank.Core.Domain.Shared.Currencies;

namespace Shreksoft.Bank.Core.Domain.Accounts.Exceptions;

public sealed class CurrencyMismatchException(CurrencyCode currencyCode, CurrencyCode currencyCodeOther)
    : Exception($"Currency {currencyCode} isn't equal {currencyCodeOther}")
{
    public CurrencyCode CurrencyCode { get; } = currencyCode;
    public CurrencyCode CurrencyCodeOther { get; } = currencyCodeOther;
}
