using CBS.Core.Accounts.Domain;
using CBS.Core.Accounts.Domain.Exceptions;
using CBS.Core.Accounts.Services;
using CBS.Core.Infrastructure.Data;
using CBS.Core.Infrastructure.Data.Accounts;
using CBS.Core.Infrastructure.Providers.Rates;

namespace CBS.Core.Tests.Accounts.Services;

public class AccountServiceTests
{
  private static AccountService CreateAccountService()
  {
    var table = new Table<Account>();
    var unitOfWork = new UnitOfWork();
    var repo = new InMemoryAccountRepository(table, unitOfWork);
    var ratesProvider = new InMemoryConvertRateProvider();

    return new AccountService(unitOfWork, repo, ratesProvider);
  }

  private static (Account, Account) CreateAccountPair(AccountService accountService)
  {
    const Currency currency = Currency.SLP;
    var money = new Money(currency, 0);
    var acc1 = accountService.CreateAccount(Guid.NewGuid(), money);
    var acc2 = accountService.CreateAccount(Guid.NewGuid(), money);
    return (acc1, acc2);
  }

  [Fact]
  public void Transfer_AmountAndAccountsCorrect_MoneyTransferredToRecipient()
  {
    var accountService = CreateAccountService();
    var (sender, recipient) = CreateAccountPair(accountService);
    const int amount = 100;

    accountService.Deposit(sender.Id, amount);
    accountService.Transfer(sender.Id, recipient.Id, amount);

    sender = accountService.GetById(sender.Id);
    recipient = accountService.GetById(recipient.Id);

    Assert.Equal(0, sender.Money.Amount);
    Assert.Equal(amount, recipient.Money.Amount);
  }

  [Fact]
  public void Transfer_RecipientIsBlocked_MoneyDidntTransfer()
  {
    var accountService = CreateAccountService();
    var (sender, recipient) = CreateAccountPair(accountService);
    const int amount = 100;
    accountService.Deposit(sender.Id, amount);
    accountService.BlockAccount(recipient.Id);

    Assert.Throws<AccountBlockedException>(() => accountService.Transfer(sender.Id, recipient.Id, amount));

    sender = accountService.GetById(sender.Id);
    recipient = accountService.GetById(recipient.Id);
    Assert.Equal(amount, sender.Money.Amount);
    Assert.Equal(0, recipient.Money.Amount);
  }

  [Fact]
  public void Transfer_RecipientAndSenderTheSame_Throws()
  {
    var accountService = CreateAccountService();
    var (sender, _) = CreateAccountPair(accountService);

    Assert.Throws<InvalidOperationException>(() => accountService.Transfer(sender.Id, sender.Id, 100));
  }

  [Theory]
  [InlineData(Currency.SLP, Currency.PIZ, 100, 1.2)]
  [InlineData(Currency.PIZ, Currency.SLP, 10, 830)]
  public void Transfer_RecipientAndSenderDiffrentCurrency_CorrectConvert(Currency currFrom, Currency currTo, decimal amountFrom, decimal amountTo)
  {
    var accountService = CreateAccountService();
    var senMoney = new Money(currFrom, amountFrom);
    var sender = accountService.CreateAccount(Guid.NewGuid(), senMoney);
    var recMoney = new Money(currTo, 0);
    var recipient = accountService.CreateAccount(Guid.NewGuid(), recMoney);

    var ex = Record.Exception(() => accountService.Transfer(sender.Id, recipient.Id, amountFrom));

    sender = accountService.GetById(sender.Id);
    recipient = accountService.GetById(recipient.Id);

    Assert.Null(ex);
    Assert.Equal(0, sender.Money.Amount);
    Assert.Equal(currFrom, sender.Money.Currency);
    Assert.Equal(amountTo, recipient.Money.Amount, precision: 2);
    Assert.Equal(currTo, recipient.Money.Currency);
  }
}
