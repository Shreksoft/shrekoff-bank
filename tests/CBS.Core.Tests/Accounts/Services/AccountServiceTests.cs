using CBS.Core.Accounts.Domain;
using CBS.Core.Accounts.Domain.Exceptions;
using CBS.Core.Accounts.Services;
using Moq;

namespace CBS.Core.Tests.Accounts.Services;

public class AccountServiceTests
{
  private readonly Mock<IAccountRepository> _accountRepository = new();
  private readonly Mock<IUnitOfWork> _unitOfWork = new();
  private readonly Mock<IConvertRateProvider> _ratesProvider = new();
  private readonly AccountService _accountService;

  public AccountServiceTests()
  {
    _accountService = new AccountService(_unitOfWork.Object, _accountRepository.Object, _ratesProvider.Object);
  }

  private (Account, Account) CreateAccountPairBypassService(Currency senderCurr = Currency.SLP, Currency recipientCurr = Currency.SLP, double rate = 1.0)
  {
    var money1 = new Money(senderCurr, 0);
    var acc1 = new Account(Guid.NewGuid(), money1);
    var money2 = new Money(recipientCurr, 0);
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

    _accountService.Transfer(sender.Id, recipient.Id, amount);

    _accountRepository.Verify(r => r.Update(sender), Times.Once());
    _accountRepository.Verify(r => r.Update(recipient), Times.Once());

    _unitOfWork.Verify(u => u.SaveChanges(), Times.Once());

    Assert.Equal(0, sender.Money.Amount);
    Assert.Equal(amount, recipient.Money.Amount);
  }

  [Fact]
  public void Transfer_RecipientIsBlocked_MoneyDidntTransfer()
  {
    var (sender, recipient) = CreateAccountPairBypassService();
    const int amount = 100;
    sender.Credit(amount);
    recipient.Block();

    Assert.Throws<AccountBlockedException>(() => _accountService.Transfer(sender.Id, recipient.Id, amount));

    _accountRepository.Verify(r => r.Update(sender), Times.Once());
    _accountRepository.Verify(r => r.Update(recipient), Times.Never());

    _unitOfWork.Verify(u => u.SaveChanges(), Times.Never());
  }

  [Fact]
  public void Transfer_RecipientAndSenderTheSame_Throws()
  {
    var (sender, _) = CreateAccountPairBypassService();

    Assert.Throws<InvalidOperationException>(() => _accountService.Transfer(sender.Id, sender.Id, 100));
  }

  [Theory]
  [InlineData(Currency.SLP, Currency.PIZ, 10, 25, 2.5)]
  [InlineData(Currency.PIZ, Currency.SLP, 10, 12, 1.2)]
  [InlineData(Currency.PIZ, Currency.SLP, 100, 50, 0.5)]
  public void Transfer_RecipientAndSenderDifferentCurrency_CorrectConvert(Currency currFrom, Currency currTo, decimal amountFrom, decimal amountTo, double rate)
  {
    var (sender, recipient) = CreateAccountPairBypassService(currFrom, currTo, rate);

    sender.Credit(amountFrom);

    var ex = Record.Exception(() => _accountService.Transfer(sender.Id, recipient.Id, amountFrom));

    _accountRepository.Verify(r => r.Update(sender), Times.Once());
    _accountRepository.Verify(r => r.Update(recipient), Times.Once());

    _unitOfWork.Verify(u => u.SaveChanges(), Times.Once());

    Assert.Null(ex);
    Assert.Equal(0, sender.Money.Amount);
    Assert.Equal(currFrom, sender.Money.Currency);
    Assert.Equal(amountTo, recipient.Money.Amount, precision: 2);
    Assert.Equal(currTo, recipient.Money.Currency);
  }
}
