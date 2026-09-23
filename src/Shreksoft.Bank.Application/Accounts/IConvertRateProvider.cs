using Shreksoft.Bank.Core.Domain.Shared.Currencies;

namespace Shreksoft.Bank.Application.Accounts;

public interface IConvertRateProvider
{
    public decimal GetRate(CurrencyCode fromCurr, CurrencyCode toCurr);
}
