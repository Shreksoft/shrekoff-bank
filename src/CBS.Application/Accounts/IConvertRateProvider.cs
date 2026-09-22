using CBS.Core.Accounts.Domain.Currencies;

namespace CBS.Application.Accounts;

public interface IConvertRateProvider
{
  public decimal GetRate(CurrencyCode fromCurr, CurrencyCode toCurr);
}
