using CBS.Application.Accounts;
using CBS.Core.Domain.Shared.Currencies;

namespace CBS.Infrastructure.Data.Providers.Rates;

public class InMemoryConvertRateProvider : IConvertRateProvider
{
  // Table relative SLP
  private readonly Dictionary<CurrencyCode, decimal> _rateTable = new()
  {
    { CurrencyCode.SLP, 1.0m },
    { CurrencyCode.PIZ, 83.0m }
  };

  public decimal GetRate(CurrencyCode fromCurr, CurrencyCode toCurr)
  {
    var (c1, c2) = GetCurrencyRateOrThrow(fromCurr, toCurr);
    return c1 / c2;
  }

  private (decimal, decimal) GetCurrencyRateOrThrow(CurrencyCode fromCurr, CurrencyCode toCurr)
  {
    if (!_rateTable.TryGetValue(fromCurr, out var fromRate) || !_rateTable.TryGetValue(toCurr, out var toRate))
      throw new ArgumentException("This currency doesn't exist in currency table");

    return (fromRate, toRate);
  }
}
