namespace CBS.Core.Services.Ledger;

public interface IAccountRepository
{
  public Account? FindById(Guid accountId);
  public IReadOnlyCollection<Account> FindByClientId(Guid clientId);
  public void Save(Account account);
}