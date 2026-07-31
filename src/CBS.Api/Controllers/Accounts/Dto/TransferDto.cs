namespace CBS.Api.Controllers.Accounts.Dto;

public record TransferDto(Guid SenderAccId, Guid RecipientAccId, decimal Amount);
