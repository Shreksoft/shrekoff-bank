using CBS.Core.Domain.Shared.Currencies;

namespace CBS.Api.Controllers.Accounts.Dto;

public record CreateAccountDto(
  Guid ClientId,
  CurrencyCode CurrencyCode
);
