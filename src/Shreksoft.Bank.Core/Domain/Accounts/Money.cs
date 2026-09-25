using Shreksoft.Bank.Core.Domain.Accounts.Exceptions;
using Shreksoft.Bank.Core.Domain.Shared.Currencies;

namespace Shreksoft.Bank.Core.Domain.Accounts;

public readonly record struct Money
{
    public decimal Amount { get; private init; }
    public Currency Currency { get; private init; }

    /// <summary>
    /// The constructor is used to validate DTOs or other unsafe inputs.
    /// </summary>
    /// <param name="currency"></param>
    /// <param name="amount"></param>
    public Money(Currency currency, decimal amount)
    {
        Validate(currency, amount);

        Amount = amount;
        Currency = currency;
    }

    /// <summary>
    /// The Round is used to immediately round the input value according to the scale from the Currency
    /// </summary>
    /// <param name="currency"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    public static Money Round(Currency currency, decimal amount) => new(currency, decimal.Round(amount, currency.Scale));

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

    private static void Validate(Currency currency, decimal amount)
    {
        if (currency.IsDefault)
            throw new ArgumentException("Currency is incorrect");

        // minorUnit = +-(mantissa) / 10^scale = 1 / 10^scale
        var minorUnit = new decimal(1, 0, 0, false, currency.Scale);
        if (decimal.Remainder(amount, minorUnit) != 0m)
            throw new ArgumentException("Incorrect amount");
    }

    // for EF
    public Money()
    {
    }
}
