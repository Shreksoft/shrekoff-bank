using CBS.Core.Accounts.Domain;
using CBS.Core.Accounts.Domain.Exceptions;
using CBS.Core.Exceptions;

namespace CBS.Core.Accounts.Services;

public class AccountService(IAccountRepository repository)
{
  public Account CreateAccount(Guid clientId, Currency currency)
  {
    var account = new Account(clientId, currency);
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

    try
    {
      sender.EnsureSameCurrency(recipient);
      sender.Debit(amount);
      recipient.Credit(amount);
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
    return account.Balance;
  }
}
