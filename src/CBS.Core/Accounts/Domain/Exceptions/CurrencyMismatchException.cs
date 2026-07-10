using CBS.Core.Accounts.Domain;

namespace CBS.Core.Accounts.Exceptions;

public sealed class CurrencyMismatchException(Currency currency, Currency currencyOther) : Exception($"Currency {currency} isn't equal {currencyOther}")
{
  Currency Currency { get; } = currency;
  Currency CurrencyOther { get; } = currencyOther;

}
