using Shreksoft.Bank.Core.Domain.Accounts.Exceptions;
using Shreksoft.Bank.Core.Domain.Shared.Currencies;

namespace Shreksoft.Bank.Core.Domain.Accounts;

public readonly record struct Money
{
    public decimal Amount { get; private init; }
    public Currency Currency { get; private init; }

    public Money(Currency currency, decimal amount)
    {
        if (currency.IsDefault)
            throw new ArgumentException("Currency is incorrect");

        var roundedAmount = decimal.Round(amount, currency.Scale);
        if (roundedAmount != amount)
            throw new ArgumentException("Incorrect amount");

        Amount = roundedAmount;
        Currency = currency;
    }

    public Money Subtract(Money money)
    {
        Currency.EnsureSameAs(money.Currency);
        return new Money(Currency, Amount - money.Amount);
    }

    public Money Add(Money money)
    {
        Currency.EnsureSameAs(money.Currency);
        return new Money(Currency, Amount + money.Amount);
    }

    // for EF
    public Money()
    {
    }
}
