namespace CBS.Core.Clients.Domain;

// record под копотом перегружает Equals, GetHashCode, ToString и операторы == != + деконструкция
public readonly record struct ClientInfo
{
  public FullName FullName { get; }
  public BirthDate BirthDate { get; }
  public string? Email { get; init; }
  public string? PhoneNumber { get; }

  public ClientInfo(FullName fullName, BirthDate birthDate, string? email, string? phoneNumber)
  {
    FullName = fullName;
    BirthDate = birthDate;
    Email = email?.ToLower();
    PhoneNumber = phoneNumber;
  }
}
