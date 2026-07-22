namespace CBS.Core.Clients.Domain;

// record под копотом перегружает Equals, GetHashCode, ToString и операторы == != + деконструкция
public readonly record struct ClientInfo
{
  public string FirstName { get; }
  public string LastName { get; }
  public BirthDate BirthDate { get; }
  public string? Email { get; init; }
  public string? PhoneNumber { get; }

  public ClientInfo(string firstName, string lastName, BirthDate birthDate, string? email, string? phoneNumber)
  {
    if (string.IsNullOrWhiteSpace(FirstName))
      throw new InvalidOperationException("FirstName can't be null or white space");

    if (string.IsNullOrWhiteSpace(LastName))
      throw new InvalidOperationException("LastName can't be null or white space");

    FirstName = firstName.ToLower();
    LastName = lastName.ToLower();
    BirthDate = birthDate;
    Email = email?.ToLower();
    PhoneNumber = phoneNumber;
  }
}
