using CBS.Api.Controllers.Accounts.Dto;
using CBS.Core.Accounts.Services;
using CBS.Core.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace CBS.Api.Controllers.Accounts;

public class AccountController : BaseApiController
{
  [HttpPost("[action]")]
  public IActionResult Create(CreateAccountDto dto, [FromServices] CreateAccountUseCase useCase)
  {
    var (clientId, currency) = dto;
    var accountId = useCase.Execute(clientId, currency);
    return CreatedAtAction(nameof(GetAccountById), new { accountId }, new { id = accountId });
  }

  [HttpGet("{accountId:guid}")]
  public IActionResult GetAccountById(Guid accountId, [FromServices] AccountService accountService)
  {
    var account = accountService.GetByIdOrThrow(accountId);
    return Ok(new { account });
  }

  [HttpPost("[action]")]
  public IActionResult Transfer(TransferDto dto, [FromServices] TransferUseCase useCase)
  {
    var (senderAccountId, recipientAccountId, amount) = dto;
    var guid = useCase.Execute(senderAccountId, recipientAccountId, amount);
    return Ok(new { guid });
  }

  [HttpPut("[action]")]
  public IActionResult Deposit(DepositDto dto, [FromServices] AccountService accountService)
  {
    var (accountId, amount) = dto;
    var balance = accountService.Deposit(accountId, amount);
    return Ok(new { balance });
  }
}
