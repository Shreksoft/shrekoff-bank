using Shreksoft.Bank.Api.Controllers.Accounts.Dto;
using Shreksoft.Bank.Application.Accounts;
using Microsoft.AspNetCore.Mvc;

namespace Shreksoft.Bank.Api.Controllers.Accounts;

public class AccountController : BaseApiController
{
    [HttpPost("[action]")]
    public IActionResult Create(CreateAccountDto dto, [FromServices] AccountService accountService)
    {
        var (clientId, currency) = dto;
        var accountId = accountService.OpenAccount(clientId, currency).Id;
        return CreatedAtAction(nameof(GetAccountById), new { accountId }, new { id = accountId });
    }

    [HttpGet("{accountId:guid}")]
    public IActionResult GetAccountById(Guid accountId, [FromServices] AccountService accountService)
    {
        var account = accountService.GetByIdOrThrow(accountId);
        return Ok(new { account });
    }

    [HttpPost("[action]")]
    public IActionResult Transfer(TransferDto dto, [FromServices] AccountService accountService)
    {
        var (senderAccountId, recipientAccountId, amount) = dto;
        var guid = accountService.Transfer(senderAccountId, recipientAccountId, amount);
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
