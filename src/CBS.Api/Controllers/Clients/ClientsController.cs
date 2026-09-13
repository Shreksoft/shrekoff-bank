using CBS.Api.Controllers.Clients.Dto;
using CBS.Application.Clients;
using CBS.Core.Clients.Domain;
using Microsoft.AspNetCore.Mvc;

namespace CBS.Api.Controllers.Clients;

public class ClientsController : BaseApiController
{
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
