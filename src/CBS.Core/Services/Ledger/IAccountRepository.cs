namespace CBS.Core.Services.Ledger;

public interface IAccountRepository
{
  public Account? Find(Guid accountId);
  public void Save(Account account);
}