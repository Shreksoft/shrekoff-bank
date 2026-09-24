using Shreksoft.Bank.Core.Domain.Accounts;
using Shreksoft.Bank.Core.Domain.Shared.Currencies;

namespace Shreksoft.Bank.Core.Domain.Transfers;

public class Transfer
{
    private const decimal CommissionRatio = 0.02m;

    public TransferSide SenderSide { get; }
    public TransferSide RecipientSide { get; }
    public Money SenderMoney { get; private set; }
    public Money RecipientMoney { get; private set; }
    public Guid Id { get; } = Guid.NewGuid();
    public Money CommissionMoney { get; private set; }
    public decimal Rate { get; private set; }

    // for EF
    private Transfer()
    {
    }

    public Transfer(TransferSide senderSide, TransferSide recipientSide, Money transferMoney, CurrencyCode recipientCurrencyCode, decimal rate)
    {
        if (senderSide.AccountId == recipientSide.AccountId)
            throw new InvalidOperationException("Transfers between the same account are prohibited");

        if (rate <= 0)
            throw new ArgumentException("Rate is less than 0 or equals 0");

        if (transferMoney.Amount <= 0)
            throw new ArgumentException("Amount for send is less than 0 or equals 0");

        SenderMoney = transferMoney;
        SenderSide = senderSide;
        RecipientSide = recipientSide;

        var recipientCurrency = new Currency(recipientCurrencyCode);
        var recipientMoney = new Money(recipientCurrency, rate * transferMoney.Amount);

        CommissionMoney = senderSide.ClientId == recipientSide.ClientId
            ? new Money(recipientCurrency, 0)
            : new Money(recipientCurrency, recipientMoney.Amount * CommissionRatio);

        RecipientMoney = recipientMoney.Subtract(CommissionMoney);
        Rate = rate;
    }
}
