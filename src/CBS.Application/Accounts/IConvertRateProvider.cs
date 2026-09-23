using CBS.Core.Domain.Shared.Currencies;

namespace CBS.Application.Accounts;

public interface IConvertRateProvider
{
  public decimal GetRate(CurrencyCode fromCurr, CurrencyCode toCurr);
}
