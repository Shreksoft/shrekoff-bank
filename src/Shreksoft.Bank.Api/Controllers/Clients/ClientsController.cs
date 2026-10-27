using Microsoft.AspNetCore.Authorization;
using Shreksoft.Bank.Api.Controllers.Clients.Dto;
using Shreksoft.Bank.Application.Clients;
using Shreksoft.Bank.Core.Domain.Clients;
using Microsoft.AspNetCore.Mvc;

namespace Shreksoft.Bank.Api.Controllers.Clients;

[Authorize(Policy = "ApiScope")]
public class ClientsController : BaseApiController
{
    [Authorize(Policy = "M2mClient")]
    [HttpPost("[action]")]
    public IActionResult Create(ClientDto dto, [FromServices] ClientService clientService)
    {
        var ci = new ClientInfo(
            new FullName(dto.FirstName, dto.MiddleName, dto.LastName),
            new BirthDate(dto.BirthDate),
            dto.Email,
            dto.PhoneNumber
        );

        var clientId = clientService.CreateClient(ci).Id;

        return CreatedAtAction(nameof(GetById), new { clientId }, new { id = clientId });
    }

    [HttpGet("{clientId:guid}")]
    public IActionResult GetById(Guid clientId, [FromServices] ClientService clientService)
    {
        var client = clientService.GetByIdOrThrow(clientId);
        return Ok(new { client });
    }
}
