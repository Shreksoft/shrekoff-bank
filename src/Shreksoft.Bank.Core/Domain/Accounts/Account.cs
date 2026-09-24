using Shreksoft.Bank.Core.Domain.Accounts.Exceptions;
using Shreksoft.Bank.Core.Domain.Shared.Currencies;

namespace Shreksoft.Bank.Core.Domain.Accounts;

public class Account
{
    public Account(Guid clientId, Money money)
    {
        if (clientId == Guid.Empty)
            throw new ArgumentException("ClientId is required", nameof(clientId));
        if (money.Currency.IsDefault)
            throw new ArgumentException("Currency is required", nameof(money));

        Id = Guid.NewGuid();
        ClientId = clientId;
        Money = money;
        IsBlocked = false;
    }

    // for EF
    private Account()
    {
    }

    public Guid Id { get; init; }
    public Guid ClientId { get; private set; }
    public Money Money { get; private set; }
    public bool IsBlocked { get; private set; }

    public void Debit(Money debitMoney)
    {
        if (debitMoney.Amount <= 0) throw new AmountIsNegativeException(Id, debitMoney.Amount);
        if (IsBlocked) throw new AccountBlockedException(Id);

        var balance = Money.Amount;
        var newBalance = Money.Subtract(debitMoney);
        if (newBalance.Amount < 0)
            throw new InsufficientFundsException(Id, balance);

        Money = newBalance;
    }

    public void Credit(Money creditMoney)
    {
        if (creditMoney.Amount <= 0) throw new AmountIsNegativeException(Id, creditMoney.Amount);
        if (IsBlocked) throw new AccountBlockedException(Id);

        Money = Money.Add(creditMoney);
    }

    public void Block()
    {
        IsBlocked = true;
    }

    public void Unblock()
    {
        IsBlocked = false;
    }
}
