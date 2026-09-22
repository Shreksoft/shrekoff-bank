using CBS.Application.Clients;
using CBS.Core.Accounts.Domain;
using CBS.Core.Accounts.Domain.Currencies;
using CBS.Application.Shared;
using CBS.Core.Domain.Transfers;

namespace CBS.Application.Accounts;

public class AccountService(
  IUnitOfWork unitOfWork,
  IAccountRepository accountRepository,
  IClientRepository clientRepository,
  ITransferRepository transferRepository,
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

  public Transfer Transfer(Guid senderAccountId, Guid recipientAccountId, decimal amount)
  {
    var sender = GetByIdOrThrow(senderAccountId);
    var recipient = GetByIdOrThrow(recipientAccountId);

    var rate = convertRateProvider.GetRate(sender.Money.Currency.Code, recipient.Money.Currency.Code);
    var senderInfo = new TransferSide(sender.Id, sender.ClientId, sender.Money.Currency);
    var recipientInfo = new TransferSide(recipient.Id, recipient.ClientId, recipient.Money.Currency);
    var transfer = new Transfer(senderInfo, recipientInfo, amount, rate);

    sender.Debit(transfer.SenderAmount);
    recipient.Credit(transfer.RecipientAmount);

    transferRepository.Add(transfer);
    unitOfWork.SaveChanges();

    return transfer;
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
}
