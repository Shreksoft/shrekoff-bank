using Shreksoft.Bank.Core.Domain.Shared.Currencies;

namespace Shreksoft.Bank.Api.Controllers.Accounts.Dto;

public record CreateAccountDto(
    Guid ClientId,
    CurrencyCode CurrencyCode
);
