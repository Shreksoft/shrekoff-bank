using CBS.Core.Accounts.Domain.Currencies;

namespace CBS.Api.Controllers.Accounts.Dto;

public record CreateAccountDto(
  Guid ClientId,
  CurrencyCode CurrencyCode
);
