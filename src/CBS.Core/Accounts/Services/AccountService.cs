using CBS.Core.Accounts.Domain;

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
    var transferId = Guid.NewGuid();
    var sender = GetAccountOrThrow(senderAccId);
    var recipient = GetAccountOrThrow(recipientAccId);

    sender.EnsureSameCurrency(recipient);
    sender.Debit(amount);
    recipient.Credit(amount);

    return transferId;
  }

  private Account GetAccountOrThrow(Guid accountId)
  {
    return repository.FindById(accountId)
                  ?? throw new InvalidOperationException($"Account {accountId} not found");
  }
}
