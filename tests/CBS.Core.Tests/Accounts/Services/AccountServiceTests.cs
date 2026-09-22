using CBS.Application;
using CBS.Application.Accounts;
using CBS.Application.Clients;
using CBS.Core.Accounts.Domain;
using CBS.Core.Accounts.Domain.Currencies;
using CBS.Core.Accounts.Domain.Exceptions;
using Moq;

namespace CBS.Core.Tests.Accounts.Services;

public class AccountServiceTests
{
  private readonly Mock<IAccountRepository> _accountRepository = new();
  private readonly Mock<IClientRepository> _clientRepository = new();
  private readonly Mock<ITransferRepository> _transferRepository = new();
  private readonly AccountService _accountService;
  private readonly Mock<IConvertRateProvider> _ratesProvider = new();
  private readonly Mock<IUnitOfWork> _unitOfWork = new();

  public AccountServiceTests()
  {
    _accountService = new AccountService(_unitOfWork.Object, _accountRepository.Object, _clientRepository.Object, _transferRepository.Object, _ratesProvider.Object);
  }

  private (Account, Account) CreateAccountPairBypassService(CurrencyCode senderCurr = CurrencyCode.SLP,
    CurrencyCode recipientCurr = CurrencyCode.SLP, decimal rate = 1.0m)
  {
    var money1 = new Money(new Currency(senderCurr), 0);
    var acc1 = new Account(Guid.NewGuid(), money1);
    var money2 = new Money(new Currency(recipientCurr), 0);
    var acc2 = new Account(Guid.NewGuid(), money2);

    _accountRepository.Setup(r => r.FindById(acc1.Id)).Returns(acc1);
    _accountRepository.Setup(r => r.FindById(acc2.Id)).Returns(acc2);
    _ratesProvider.Setup(r => r.GetRate(senderCurr, recipientCurr)).Returns(rate);

    return (acc1, acc2);
  }

  [Fact]
  public void Transfer_AmountAndAccountsCorrect_MoneyTransferredToRecipient()
  {
    const int amount = 100;
    var (sender, recipient) = CreateAccountPairBypassService();
    sender.Credit(amount);

    var transfer = _accountService.Transfer(sender.Id, recipient.Id, amount);

    _unitOfWork.Verify(u => u.SaveChanges(), Times.Once());

    Assert.Equal(0, sender.Money.Amount);
    Assert.Equal(transfer.RecipientAmount, recipient.Money.Amount);
  }

  [Fact]
  public void Transfer_RecipientIsBlocked_MoneyDidntTransfer()
  {
    var (sender, recipient) = CreateAccountPairBypassService();
    const int amount = 100;
    sender.Credit(amount);
    recipient.Block();

    Assert.Throws<AccountBlockedException>(() => _accountService.Transfer(sender.Id, recipient.Id, amount));

    _unitOfWork.Verify(u => u.SaveChanges(), Times.Never());
  }

  [Fact]
  public void Transfer_RecipientAndSenderTheSame_Throws()
  {
    var (sender, _) = CreateAccountPairBypassService();

    Assert.Throws<InvalidOperationException>(() => _accountService.Transfer(sender.Id, sender.Id, 100));
  }

  [Theory]
  [InlineData(CurrencyCode.SLP, CurrencyCode.PIZ, 10, 25 - 25 * 0.02, 2.5)]
  [InlineData(CurrencyCode.PIZ, CurrencyCode.SLP, 10, 12 - 12 * 0.02, 1.2)]
  [InlineData(CurrencyCode.PIZ, CurrencyCode.SLP, 100, 50 - 50 * 0.02, 0.5)]
  public void Transfer_DifferentCurrency_CorrectConvert(CurrencyCode currFrom, CurrencyCode currTo,
    decimal amountFrom, decimal amountTo, decimal rate)
  {
    var (sender, recipient) = CreateAccountPairBypassService(currFrom, currTo, rate);

    sender.Credit(amountFrom);

    var ex = Record.Exception(() => _accountService.Transfer(sender.Id, recipient.Id, amountFrom));

    _unitOfWork.Verify(u => u.SaveChanges(), Times.Once());

    Assert.Null(ex);
    Assert.Equal(0, sender.Money.Amount);
    Assert.Equal(currFrom, sender.Money.Currency.Code);
    Assert.Equal(amountTo, recipient.Money.Amount);
    Assert.Equal(currTo, recipient.Money.Currency.Code);
  }

  [Theory]
  [InlineData(CurrencyCode.PIZ, CurrencyCode.SLP, 100, 50, 0.5)]
  [InlineData(CurrencyCode.PIZ, CurrencyCode.SLP, 100, 30, 0.3)]
  public void Transfer_DifferentCurrency_CorrectReversedConvert(CurrencyCode senderCurrencyCode,
    CurrencyCode recipientCurrencyCode, decimal senderAmount,
    decimal recipientAmount, decimal rate)
  {
    var (sender, recipient) = CreateAccountPairBypassService(senderCurrencyCode, recipientCurrencyCode, rate);
    sender.Credit(senderAmount);

    _ratesProvider.Setup(r => r.GetRate(recipientCurrencyCode, senderCurrencyCode)).Returns(1 / rate);
    var transferTo = _accountService.Transfer(sender.Id, recipient.Id, senderAmount);
    var transferFrom = _accountService.Transfer(recipient.Id, sender.Id, recipientAmount - transferTo.Commission);
    _unitOfWork.Verify(u => u.SaveChanges(), Times.Exactly(2));

    Assert.Equal(transferFrom.RecipientAmount, sender.Money.Amount);
    Assert.Equal(0, recipient.Money.Amount);
  }
}
