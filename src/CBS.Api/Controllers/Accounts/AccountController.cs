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
    try
    {
      var (clientId, currency) = dto;
      var accountId = useCase.Execute(clientId, currency);
      return CreatedAtAction(nameof(GetAccountById), new { accountId }, new { id = accountId });
    }
    catch (Exception e)
    {
      return BadRequest(new ProblemDetails { Title = "Invalid input", Detail = e.Message });
    }
  }

  [HttpGet("{accountId:guid}")]
  public IActionResult GetAccountById(Guid accountId, [FromServices] AccountService accountService)
  {
    try
    {
      var account = accountService.GetAccountOrThrow(accountId);
      return Ok(new { account });
    }
    catch (InvalidOperationException)
    {
      return NotFound();
    }
  }

  [HttpPost("[action]")]
  public IActionResult Transfer(TransferDto dto, [FromServices] TransferUseCase useCase)
  {
    try
    {
      var (senderAccountId, recipientAccountId, amount) = dto;
      var guid = useCase.Execute(senderAccountId, recipientAccountId, amount);
      return Ok(new { guid });
    }
    catch (Exception e)
    {
      return BadRequest(new ProblemDetails { Detail = e.Message });
    }
  }

  [HttpPut("[action]")]
  public IActionResult Deposit(DepositDto dto, [FromServices] AccountService accountService)
  {
    try
    {
      var (accountId, amount) = dto;
      var balance = accountService.Deposit(accountId, amount);
      return Ok(new { balance });
    }
    catch (Exception e)
    {
      return BadRequest(new ProblemDetails { Detail = e.Message });
    }
  }
}
