using CBS.Core.Accounts.Domain;
using CBS.Core.Accounts.Domain.Exceptions;

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
    var account = GetAccountOrThrow(accountId);
    account.Unblock();
  }

  public void CloseAccount(Guid accountId)
  {
    var account = GetAccountOrThrow(accountId);
    account.Block();
  }

  public Guid Transfer(Guid senderAccId, Guid recipientAccId, decimal amount)
  {
    var sender = GetAccountOrThrow(senderAccId);
    var recipient = GetAccountOrThrow(recipientAccId);

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

  private Account GetAccountOrThrow(Guid accountId)
  {
    return repository.FindById(accountId)
                  ?? throw new InvalidOperationException($"Account {accountId} not found");
  }
}
