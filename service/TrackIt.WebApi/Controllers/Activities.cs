using TrackIt.Commands.ActivityGroupCommands.CreateActivityGroup;
using TrackIt.Infraestructure.Web.Swagger.Annotations;
using TrackIt.Infraestructure.Web.Controller;
using Microsoft.AspNetCore.Mvc;
using MediatR;

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
}