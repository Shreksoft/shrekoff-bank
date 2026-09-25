using Shreksoft.Bank.Core.Domain.Accounts.Exceptions;

namespace Shreksoft.Bank.Core.Domain.Shared.Currencies;

public readonly record struct Currency
{
    public CurrencyCode Code { get; private init; }

    public bool IsDefault => Code == default;

    public Currency(CurrencyCode code)
    {
        if (!Enum.IsDefined(code))
            throw new ArgumentOutOfRangeException(nameof(code));

        Code = code;
    }

    public byte Scale => Code switch
    {
        CurrencyCode.SLP => 2,
        _ => 5
    };

    public void EnsureSameAs(Currency other)
    {
        if (Code != other.Code)
            throw new CurrencyMismatchException(Code, other.Code);
    }
}
