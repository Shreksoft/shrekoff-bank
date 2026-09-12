using CBS.Core.Accounts.Domain.Currencies;

namespace CBS.Core.Accounts.Domain.Exceptions;

public sealed class CurrencyMismatchException(CurrencyCode currencyCode, CurrencyCode currencyCodeOther)
  : Exception($"Currency {currencyCode} isn't equal {currencyCodeOther}")
{
  public CurrencyCode CurrencyCode { get; } = currencyCode;
  public CurrencyCode CurrencyCodeOther { get; } = currencyCodeOther;
}
