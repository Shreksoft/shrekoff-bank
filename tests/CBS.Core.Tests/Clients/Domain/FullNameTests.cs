using CBS.Core.Clients.Domain;

namespace CBS.Core.Tests.Clients.Domain;

public class FullNameTests
{
  [Fact]
  public void Constructor_WhenCallWithoutArgs_FieldsAreNull()
  {
    FullName fullName = default;

    Assert.True(fullName.FirstName is null);
    Assert.True(fullName.LastName is null);
    Assert.True(fullName.MiddleName is null);
  }

  [Theory]
  [InlineData("иван", "иванович", "иванов", "Иван", "Иванович", "Иванов")]
  [InlineData("ИВАН", "ИВАНОВИЧ", "ИВАНОВ", "Иван", "Иванович", "Иванов")]
  [InlineData("иВАН", "иВАНОВИЧ", "иВАНОВ", "Иван", "Иванович", "Иванов")]
  [InlineData("   ивАН ", " иванОВИЧ", "ИваНов ", "Иван", "Иванович", "Иванов")]
  [InlineData("д'артаньян", "", "шарль", "Д'артаньян", null, "Шарль")]
  public void Constructor_DataInIncorrectRegister_NormalizesToTitleCase(string firstName, string middleName, string lastName, string expectedFirstName, string? expectedMiddleName, string expectedLastName)
  {
    var fullName = new FullName(firstName, middleName, lastName);

    Assert.Equal(expectedFirstName, fullName.FirstName);
    Assert.Equal(expectedMiddleName, fullName.MiddleName);
    Assert.Equal(expectedLastName, fullName.LastName);
  }
}
