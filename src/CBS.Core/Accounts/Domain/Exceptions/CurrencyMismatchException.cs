namespace CBS.Core.Accounts.Domain.Exceptions;

public sealed class CurrencyMismatchException(Currency currency, Currency currencyOther)
  : Exception($"Currency {currency} isn't equal {currencyOther}")
{
  public Currency Currency { get; } = currency;
  public Currency CurrencyOther { get; } = currencyOther;
}
