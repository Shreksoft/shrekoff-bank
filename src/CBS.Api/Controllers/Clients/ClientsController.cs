using CBS.Api.Controllers.Clients.Dto;
using CBS.Core.Clients.Domain;
using CBS.Core.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace CBS.Api.Controllers.Clients;

public class ClientsController : BaseApiController
{
  [HttpPost("[action]")]
  public IActionResult Create(ClientDto dto, [FromServices] CreateClientUseCase useCase)
  {
    try
    {
      var ci = new ClientInfo(
        new FullName(dto.FirstName, dto.MiddleName, dto.LastName),
        new BirthDate(dto.BirthDate),
        dto.Email,
        dto.PhoneNumber
      );

      var clientId = useCase.Execute(ci);

      return CreatedAtAction(nameof(GetById), new { clientId }, new { id = clientId });
    }
    catch (Exception e)
    {
      return BadRequest(new ProblemDetails { Title = "Invalid input", Detail = e.Message });
    }
  }

  [HttpGet("{clientId:guid}")]
  public IActionResult GetById(Guid clientId, [FromServices] GetClientByIdUseCase useCase)
  {
    try
    {
      var client = useCase.Execute(clientId);
      if (client is null) return NotFound();
      return Ok(new { client });
    }
    catch (Exception e)
    {
      return BadRequest(new ProblemDetails { Detail = e.Message, Status = StatusCodes.Status400BadRequest });
    }
  }
}
