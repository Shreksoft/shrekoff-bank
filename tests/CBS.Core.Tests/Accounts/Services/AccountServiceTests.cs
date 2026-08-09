using CBS.Core.Accounts.Domain;
using CBS.Core.Accounts.Domain.Exceptions;
using CBS.Core.Accounts.Infrastructure;
using CBS.Core.Accounts.Services;

namespace CBS.Core.Tests.Accounts.Services;

public class AccountServiceTests
{
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
    var accountService = new AccountService(new InMemoryAccountRepository(), new InMemoryConvertRateProvider());
    var (acc1, acc2) = CreateAccountPair(accountService);
    const int amount = 100;
    acc1.Credit(amount);

    accountService.Transfer(acc1.Id, acc2.Id, amount);
    Assert.Equal(0, acc1.Money.Amount);
    Assert.Equal(amount, acc2.Money.Amount);
  }

  [Fact]
  public void Transfer_RecipientIsBlocked_MoneyDidntTransfer()
  {
    var accountService = new AccountService(new InMemoryAccountRepository(), new InMemoryConvertRateProvider());
    var (acc1, acc2) = CreateAccountPair(accountService);
    const int amount = 100;
    acc1.Credit(amount);
    acc2.Block();

    Assert.Throws<AccountBlockedException>(() => accountService.Transfer(acc1.Id, acc2.Id, amount));
    Assert.Equal(amount, acc1.Money.Amount);
    Assert.Equal(0, acc2.Money.Amount);
  }

  [Fact]
  public void Transfer_RecipientAndSenderTheSame_Throws()
  {
    var accountService = new AccountService(new InMemoryAccountRepository(), new InMemoryConvertRateProvider());
    var (sender, _) = CreateAccountPair(accountService);

    Assert.Throws<InvalidOperationException>(() => accountService.Transfer(sender.Id, sender.Id, 100));
  }

  [Theory]
  [InlineData(Currency.SLP, Currency.PIZ, 100, 1.2)]
  [InlineData(Currency.PIZ, Currency.SLP, 10, 830)]
  public void TrasferRecipientAndSenderDiffrentCurrency_CorrectConvert(Currency currFrom, Currency currTo, decimal amountFrom, decimal amountTo)
  {
    var accountService = new AccountService(new InMemoryAccountRepository(), new InMemoryConvertRateProvider());

    var senMoney = new Money(currFrom, amountFrom);
    var sender = accountService.CreateAccount(Guid.NewGuid(), senMoney);
    var recMoney = new Money(currTo, 0);
    var recipient = accountService.CreateAccount(Guid.NewGuid(), recMoney);

    var ex = Record.Exception(() => accountService.Transfer(sender.Id, recipient.Id, amountFrom));

    Assert.Null(ex);
    Assert.Equal(0, sender.Money.Amount);
    Assert.Equal(currFrom, sender.Money.Currency);
    Assert.Equal(amountTo, recipient.Money.Amount, precision: 2);
    Assert.Equal(currTo, recipient.Money.Currency);
  }
}
