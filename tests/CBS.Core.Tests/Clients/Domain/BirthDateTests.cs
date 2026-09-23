using CBS.Core.Domain.Clients;

namespace CBS.Core.Tests.Clients.Domain;

public class BirthDateTests
{
  [Fact]
  public void Constructor_DateInFuture_Throws()
  {
    var date = DateOnly.MaxValue;
    Assert.Throws<ArgumentException>(() => new BirthDate(date));
  }

  [Fact]
  public void Age_BirthdayAlreadyPassedThisYear_ReturnsCorrectAge()
  {
    var date = DateOnly.FromDateTime(DateTime.Now).AddYears(-1).AddDays(-1);
    var birthDate = new BirthDate(date);
    Assert.Equal(1, birthDate.Age);
  }

  [Fact]
  public void Age_BirthdayHasntPassedThisYear_ReturnsCorrectAge()
  {
    var date = DateOnly.FromDateTime(DateTime.Now).AddYears(-1).AddDays(1);
    var birthDate = new BirthDate(date);
    Assert.Equal(0, birthDate.Age);
  }
}
