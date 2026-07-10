namespace CBS.Core.Clients.Domain;

// record под копотом перегружает Equals, GetHashCode, ToString и операторы == != + деконструкция
public readonly record struct ClientInfo(string FirstName, string LastName, DateOnly BirthDate, string? Email, string? PhoneNumber);

class Client
{
  public ClientInfo Info { get; private set; }
  public Guid Id { get; } = Guid.NewGuid();
  public DateTime CreatedDate { get; } = DateTime.Now;

  public Client(ClientInfo clientInfo)
  {
    var convertedCreatedDate = DateOnly.FromDateTime(CreatedDate);

    if (clientInfo.BirthDate > convertedCreatedDate)
      throw new ArgumentException("The client isn't born yet");

    Info = clientInfo;
  }

  public void ChangeEmail(string newEmail)
  {
    Info = Info with { Email = newEmail };
  }
}
