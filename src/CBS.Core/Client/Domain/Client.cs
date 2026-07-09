namespace CBS.Core.Client.Domain;

// record под копотом перегружает Equals, GetHashCode, ToString и операторы == != + деконструкция
public readonly record struct ClientInfo(string FirstName, string LastName, DateOnly BirthDate, string? Email, string? PhoneNumber);

class Client(ClientInfo clientInfo)
{
  public ClientInfo Info { get; private set; } = clientInfo;
  public Guid Id { get; } = Guid.NewGuid();
  public DateTime CreatedDate { get; } = DateTime.Now;

  public void ChangeEmail(string newEmail)
  {
    Info = Info with { Email = newEmail };
  }
}
