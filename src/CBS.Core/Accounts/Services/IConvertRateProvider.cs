using CBS.Core.Accounts.Domain;

namespace CBS.Core.Accounts.Services;

public interface IConvertRateProvider
{
  public double GetRate(Currency fromCurr, Currency toCurr);
}
