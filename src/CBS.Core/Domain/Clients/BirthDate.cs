namespace CBS.Core.Clients.Domain;

public readonly struct BirthDate
{
  public DateOnly Date { get; }
  public int Age => CalcAge(Date, DateOnly.FromDateTime(DateTime.Now));

  public BirthDate(DateOnly date)
  {
    if (date > DateOnly.FromDateTime(DateTime.Now))
      throw new ArgumentException("Date can't be more than date now");

    Date = date;
  }

  private static int CalcAge(DateOnly birthDate, DateOnly today)
  {
    var age = today.Year - birthDate.Year;

    if (birthDate.DayOfYear > today.DayOfYear)
      age--;

    return age;
  }
}
