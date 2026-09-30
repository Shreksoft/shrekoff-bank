using Shreksoft.Bank.Application.Clients;
using Shreksoft.Bank.Application.Shared;
using Shreksoft.Bank.Core.Domain.Accounts;
using Shreksoft.Bank.Core.Domain.Shared.Currencies;
using Shreksoft.Bank.Core.Domain.Transfers;

namespace Shreksoft.Bank.Application.Accounts;

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

        var transferMoney = new Money(sender.Money.Currency, amount);

        var rate = convertRateProvider.GetRate(sender.Money.Currency.Code, recipient.Money.Currency.Code);
        var senderInfo = new TransferSide(sender.Id, sender.ClientId);
        var recipientInfo = new TransferSide(recipient.Id, recipient.ClientId);
        var transfer = new Transfer(senderInfo, recipientInfo, transferMoney, recipient.Money.Currency.Code, rate);

        sender.Debit(transfer.SenderMoney);
        recipient.Credit(transfer.RecipientMoney);

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
        var depositMoney = new Money(account.Money.Currency, amount);

        account.Credit(depositMoney);

        unitOfWork.SaveChanges();

        return account.Money.Amount;
    }
}
