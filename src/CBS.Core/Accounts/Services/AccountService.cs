using CBS.Core.Accounts.Domain;
using CBS.Core.Accounts.Domain.Currencies;
using CBS.Core.Exceptions;

namespace CBS.Core.Accounts.Services;

public class AccountService(
  IUnitOfWork unitOfWork,
  IAccountRepository repository,
  IConvertRateProvider convertRateProvider)
{
  public Account CreateAccount(Guid clientId, Money money)
  {
    var account = new Account(clientId, money);
    repository.Add(account);
    unitOfWork.SaveChanges();
    return account;
  }

  public void OpenAccount(Guid accountId)
  {
    var account = GetById(accountId);
    account.Unblock();
    repository.Update(account);
    unitOfWork.SaveChanges();
  }

  public void BlockAccount(Guid accountId)
  {
    var account = GetById(accountId);
    account.Block();
    repository.Update(account);
    unitOfWork.SaveChanges();
  }

  public Guid Transfer(Guid senderId, Guid recipientId, decimal amount)
  {
    if (senderId == recipientId)
      throw new InvalidOperationException("Transfers between the same account are prohibited");

    var sender = GetById(senderId);
    var recipient = GetById(recipientId);

    var convertedAmount = ConvertAmount(sender.Money.Currency, recipient.Money.Currency, amount);

    sender.Debit(amount);
    repository.Update(sender);
    recipient.Credit(convertedAmount);
    repository.Update(recipient);
    unitOfWork.SaveChanges();

    var transferId = Guid.NewGuid();
    return transferId;
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
    repository.Update(account);
    unitOfWork.SaveChanges();
    return account.Money.Amount;
  }

  private decimal ConvertAmount(Currency senderCurrency, Currency recipientCurrency, decimal amount)
  {
    var rate = convertRateProvider.GetRate(senderCurrency.Code, recipientCurrency.Code);
    var convertedAmount = (decimal)rate * amount;
    return convertedAmount;
  }
}
