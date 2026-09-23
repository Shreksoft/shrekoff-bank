using Shreksoft.Bank.Core.Domain.Accounts.Exceptions;
using Shreksoft.Bank.Core.Domain.Shared.Currencies;

namespace Shreksoft.Bank.Core.Domain.Accounts;

public class Account
{
    public Account(Guid clientId, Money money)
    {
        if (!Currency.IsValid(money.Currency))
            throw new ArgumentException("Invalid currency");

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

    public void Debit(decimal amount)
    {
        if (amount <= 0) throw new AmountIsNegativeException(Id, amount);
        if (IsBlocked) throw new AccountBlockedException(Id);

        var balance = Money.Amount;
        if (amount > balance) throw new InsufficientFundsException(Id, balance);

        var newAmount = Math.Round(balance - amount, Money.Currency.Scale);
        Money = Money with { Amount = newAmount };
    }

    public void Credit(decimal amount)
    {
        if (amount <= 0) throw new AmountIsNegativeException(Id, amount);
        if (IsBlocked) throw new AccountBlockedException(Id);

        var newAmount = Math.Round(Money.Amount + amount, Money.Currency.Scale);
        Money = Money with { Amount = newAmount };
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
