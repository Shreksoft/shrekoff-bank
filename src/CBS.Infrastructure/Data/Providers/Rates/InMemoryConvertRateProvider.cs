using CBS.Core.Accounts.Domain.Currencies;
using CBS.Core.Accounts.Services;

namespace CBS.Infrastructure.Data.Providers.Rates;

public class InMemoryConvertRateProvider : IConvertRateProvider
{
  // Table relative SLP
  private readonly Dictionary<CurrencyCode, double> _rateTable = new()
  {
    { CurrencyCode.SLP, 1.0 },
    { CurrencyCode.PIZ, 83.0 }
  };

  public double GetRate(CurrencyCode fromCurr, CurrencyCode toCurr)
  {
    var (c1, c2) = GetCurrencyRateOrThrow(fromCurr, toCurr);
    return c1 / c2;
  }

  private (double, double) GetCurrencyRateOrThrow(CurrencyCode fromCurr, CurrencyCode toCurr)
  {
    if (!_rateTable.TryGetValue(fromCurr, out var fromRate) || !_rateTable.TryGetValue(toCurr, out var toRate))
      throw new ArgumentException("This currency doesn't exist in currency table");

    return (fromRate, toRate);
  }
}
