using CBS.Core.Accounts.Domain;
using CBS.Core.Accounts.Domain.Exceptions;
using CBS.Core.Exceptions;

namespace CBS.Core.Accounts.Services;

public class AccountService(IAccountRepository repository, IConvertRateProvider convertRateProvider)
{
  public Account CreateAccount(Guid clientId, Money money)
  {
    var account = new Account(clientId, money);
    repository.Save(account);
    return account;
  }

  public void OpenAccount(Guid accountId)
  {
    var account = GetById(accountId);
    account.Unblock();
  }

  public void CloseAccount(Guid accountId)
  {
    var account = GetById(accountId);
    account.Block();
  }

  public Guid Transfer(Guid senderAccId, Guid recipientAccId, decimal amount)
  {
    if (senderAccId == recipientAccId)
      throw new InvalidOperationException("Transfers between the same account are prohibited");

    var sender = GetById(senderAccId);
    var recipient = GetById(recipientAccId);
    var senderCurr = sender.Money.Currency;
    var recipientCurr = recipient.Money.Currency;

    try
    {
      var rate = convertRateProvider.GetRate(senderCurr, recipientCurr);
      var convertedAmount = (decimal)rate * amount;

      sender.Debit(amount);
      recipient.Credit(convertedAmount);
    }
    catch (AccountBlockedException ex) when (ex.AccountId == recipient.Id)
    {
      Rollback(sender, amount);
      throw;
    }

    var transferId = Guid.NewGuid();
    return transferId;
  }

  private static void Rollback(Account account, decimal amount)
  {
    try
    {
      account.Credit(amount);
    }
    catch (AccountBlockedException)
    {
      account.Unblock();
      account.Credit(amount);
      account.Block();
    }
  }

  public Account GetById(Guid accountId)
  {
    return repository.FindById(accountId)
           ?? throw new ObjectNotFoundException(accountId);
  }

  public decimal Deposit(Guid accountId, decimal amount)
  {
    var account = GetById(accountId);
    account.Credit(amount);
    return account.Money.Amount;
  }
}
