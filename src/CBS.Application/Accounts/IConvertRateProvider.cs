using CBS.Core.Accounts.Domain.Currencies;

namespace CBS.Application.Accounts;

public interface IConvertRateProvider
{
  public double GetRate(CurrencyCode fromCurr, CurrencyCode toCurr);
}
