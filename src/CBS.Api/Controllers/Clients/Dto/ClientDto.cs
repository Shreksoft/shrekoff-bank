namespace CBS.Api.Controllers.Clients.Dto;

public record ClientDto(
  string FirstName,
  string? MiddleName,
  string LastName,
  DateOnly BirthDate,
  string? Email,
  string? PhoneNumber
);
