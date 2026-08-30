using CBS.Core.Accounts.Domain;
using CBS.Core.Accounts.Services;

namespace CBS.Infrastructure.Data.Providers.Rates;

public class InMemoryConvertRateProvider : IConvertRateProvider
{
  // Table relative SLP
  private readonly Dictionary<Currency, double> _table = new()
  {
    { Currency.SLP, 1.0 },
    { Currency.PIZ, 83.0 }
  };

  public double GetRate(Currency fromCurr, Currency toCurr)
  {
    var (c1, c2) = GetCurrencyCoefsOrThrow(fromCurr, toCurr);
    return c1 / c2;
  }

  private (double, double) GetCurrencyCoefsOrThrow(Currency fromCurr, Currency toCurr)
  {
    if (!_table.TryGetValue(fromCurr, out var fromCoef) || !_table.TryGetValue(toCurr, out var toCoef))
    {
      throw new ArgumentException("This currency doesn't exist in currency table");
    }

    return (fromCoef, toCoef);
  }
}
