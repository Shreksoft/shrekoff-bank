using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shreksoft.Bank.Core.Domain.Accounts;

namespace Shreksoft.Bank.Infrastructure.Data.Accounts;

public static class MoneyConfiguration
{
    public static void Configure(ComplexPropertyBuilder<Money> money)
    {
        money.Property(m => m.Amount);
        money.ComplexProperty(m => m.Currency, currency =>
        {
            currency.Ignore(c => c.Scale);
            currency.Property(c => c.Code).HasConversion<string>();
        });
    }
}
