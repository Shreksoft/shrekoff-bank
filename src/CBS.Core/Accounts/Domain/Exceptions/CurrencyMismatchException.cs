namespace CBS.Core.Accounts.Domain.Exceptions;

public sealed class CurrencyMismatchException(Currency currency, Currency currencyOther) : Exception($"Currency {currency} isn't equal {currencyOther}")
{
  Currency Currency { get; } = currency;
  Currency CurrencyOther { get; } = currencyOther;

}
