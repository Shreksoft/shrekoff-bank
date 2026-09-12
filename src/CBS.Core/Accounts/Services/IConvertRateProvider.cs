using CBS.Core.Accounts.Domain.Currencies;

namespace CBS.Core.Accounts.Services;

public interface IConvertRateProvider
{
  public double GetRate(CurrencyCode fromCurr, CurrencyCode toCurr);
}
