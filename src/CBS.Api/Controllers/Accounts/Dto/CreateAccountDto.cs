using CBS.Core.Accounts.Domain;

namespace CBS.Api.Controllers.Accounts.Dto;

public record CreateAccountDto(
  Guid ClientId,
  Currency Currency
);
