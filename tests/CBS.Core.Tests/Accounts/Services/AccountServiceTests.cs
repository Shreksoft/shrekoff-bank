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
    var acc1 = accountService.CreateAccount(Guid.NewGuid(), currency);
    var acc2 = accountService.CreateAccount(Guid.NewGuid(), currency);
    return (acc1, acc2);
  }

  [Fact]
  public void Transfer_AmountAndAccountsCorrect_MoneyTransferredToRecipient()
  {
    var accountService = new AccountService(new InMemoryAccountRepository());
    var (acc1, acc2) = CreateAccountPair(accountService);
    const int amount = 100;
    acc1.Credit(amount);

    accountService.Transfer(acc1.Id, acc2.Id, amount);
    Assert.Equal(0, acc1.Balance);
    Assert.Equal(amount, acc2.Balance);
  }

  [Fact]
  public void Transfer_RecipientIsBlocked_MoneyDidntTransfer()
  {
    var accountService = new AccountService(new InMemoryAccountRepository());
    var (acc1, acc2) = CreateAccountPair(accountService);
    const int amount = 100;
    acc1.Credit(amount);
    acc2.Block();

    Assert.Throws<AccountBlockedException>(() => accountService.Transfer(acc1.Id, acc2.Id, amount));
    Assert.Equal(amount, acc1.Balance);
    Assert.Equal(0, acc2.Balance);
  }

  [Fact]
  public void Transfer_RecipientAndSenderTheSame_Throws()
  {
    var accountService = new AccountService(new InMemoryAccountRepository());
    var (sender, _) = CreateAccountPair(accountService);

    Assert.Throws<InvalidOperationException>(() => accountService.Transfer(sender.Id, sender.Id, 100));
  }
}
