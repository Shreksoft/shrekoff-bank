namespace CBS.Core.Accounts.Domain.Currencies;

public readonly record struct Currency(CurrencyCode Code)
{
  public byte Scale { get; } = GetScale(Code);

  private static byte GetScale(CurrencyCode code)
  {
    return code switch
    {
      CurrencyCode.SLP => 2,
      _ => 5
    };
  }

  public static bool IsValid(Currency currency)
  {
    return Enum.IsDefined(currency.Code) && currency.Scale == GetScale(currency.Code);
  }
}
