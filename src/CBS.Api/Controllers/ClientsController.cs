using CBS.Core.Clients.Domain;
using CBS.Core.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace CBS.Api.Controllers;

public class ClientsController : BaseApiController
{
  [HttpPost("[action]")]
  public IActionResult Create(CreateClientRequest request, [FromServices] CreateClientUseCase useCase)
  {
    try
    {
      var ci = new ClientInfo(
        new FullName(request.FirstName, request.MiddleName, request.LastName),
        new BirthDate(request.BirthDate),
        request.Email,
        request.PhoneNumber
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

public record CreateClientRequest(
  string FirstName,
  string? MiddleName,
  string LastName,
  DateOnly BirthDate,
  string? Email,
  string? PhoneNumber
);
