namespace CBS.Core.Clients.Domain;

// record под копотом перегружает Equals, GetHashCode, ToString и операторы == != + деконструкция
public readonly record struct ClientInfo
{
  public FullName FullName { get; init; }
  public BirthDate BirthDate { get; init; }
  public string? Email { get; init; }
  public string? PhoneNumber { get; init; }

  public ClientInfo(FullName fullName, BirthDate birthDate, string? email, string? phoneNumber)
  {
    FullName = fullName;
    BirthDate = birthDate;
    Email = email?.ToLower();
    PhoneNumber = phoneNumber;
  }

  // for EFCore
  public ClientInfo() : this(default, default, null, null) { }
}
