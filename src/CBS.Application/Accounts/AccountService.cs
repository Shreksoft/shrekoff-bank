using CBS.Application.Clients;
using CBS.Core.Accounts.Domain;
using CBS.Core.Accounts.Domain.Currencies;
using CBS.Application.Shared;

namespace CBS.Application.Accounts;

public class AccountService(
  IUnitOfWork unitOfWork,
  IAccountRepository accountRepository,
  IClientRepository clientRepository,
  IConvertRateProvider convertRateProvider)
{
  public Account OpenAccount(Guid clientId, CurrencyCode currencyCode)
  {
    if (clientRepository.FindById(clientId) is null)
      throw new ObjectNotFoundException(clientId);

    var money = new Money(new Currency(currencyCode), 0);
    var account = new Account(clientId, money);
    accountRepository.Add(account);
    unitOfWork.SaveChanges();
    return account;
  }

  public Guid Transfer(Guid senderId, Guid recipientId, decimal amount)
  {
    if (senderId == recipientId)
      throw new InvalidOperationException("Transfers between the same account are prohibited");

    var sender = GetByIdOrThrow(senderId);
    var recipient = GetByIdOrThrow(recipientId);

    var convertedAmount = ConvertAmount(sender.Money.Currency, recipient.Money.Currency, amount);

    sender.Debit(amount);
    recipient.Credit(convertedAmount);

    unitOfWork.SaveChanges();

    var transferId = Guid.NewGuid();
    return transferId;
  }

  public Account GetByIdOrThrow(Guid accountId)
  {
    return accountRepository.FindById(accountId)
           ?? throw new ObjectNotFoundException(accountId);
  }

  public decimal Deposit(Guid accountId, decimal amount)
  {
    var account = GetByIdOrThrow(accountId);
    account.Credit(amount);
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
