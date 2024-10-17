using TrackIt.Commands.ActivityGroupCommands.CreateActivityGroup;
using TrackIt.Infraestructure.Web.Swagger.Annotations;
using TrackIt.Infraestructure.Web.Controller;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using TrackIt.Commands.ActivityGroupCommands.UpdateActivityGroup;

namespace TrackIt.WebApi.Controllers;

[Tags("Activity")]
[Route("activity")]
[ApiController]
public class Activities : BaseController
{
  private readonly IMediator _mediator;

  public Activities (IMediator mediator) => _mediator = mediator;

  [HttpPost("group")]
  [SwaggerAuthorize]
  public async Task<IActionResult> Handle ([FromBody] CreateActivityGroupPayload payload)
  {
    await _mediator.Send(new CreateActivityGroupCommand(payload, SessionFromHeaders()));
    
    return StatusCode(201);
  }

  [HttpPut("group/{id}")]
  [SwaggerAuthorize]
  public async Task<IActionResult> Handle (Guid id, [FromBody] UpdateActivityGroupPayload payload)
  {
    await _mediator.Send(new UpdateActivityGroupCommand(id, payload, SessionFromHeaders()));
    
    return Ok();
  }
}