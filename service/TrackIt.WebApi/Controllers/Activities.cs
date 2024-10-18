using TrackIt.Commands.ActivityGroupCommands.CreateActivityGroup;
using TrackIt.Commands.ActivityGroupCommands.UpdateActivityGroup;
using TrackIt.Commands.ActivityGroupCommands.DeleteActivityGroup;
using TrackIt.Commands.ActivityCommands.CreateActivity;
using TrackIt.Commands.ActivityCommands.UpdateActivity;
using TrackIt.Infraestructure.Web.Swagger.Annotations;
using TrackIt.Infraestructure.Web.Controller;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace TrackIt.WebApi.Controllers;

[Tags("Activity")]
[Route("group")]
[ApiController]
public class Activities : BaseController
{
  private readonly IMediator _mediator;

  public Activities (IMediator mediator) => _mediator = mediator;

  [HttpPost]
  [SwaggerAuthorize]
  public async Task<IActionResult> Handle ([FromBody] CreateActivityGroupPayload payload)
  {
    await _mediator.Send(new CreateActivityGroupCommand(payload, SessionFromHeaders()));
    
    return StatusCode(201);
  }

  [HttpPut("{id}")]
  [SwaggerAuthorize]
  public async Task<IActionResult> Handle (Guid id, [FromBody] UpdateActivityGroupPayload payload)
  {
    await _mediator.Send(new UpdateActivityGroupCommand(id, payload, SessionFromHeaders()));
    
    return Ok();
  }

  [HttpDelete("{id}")]
  [SwaggerAuthorize]
  public async Task<IActionResult> Handle (Guid id)
  {
    await _mediator.Send(new DeleteActivityGroupCommand(id, SessionFromHeaders()));
    
    return Ok();
  }

  [HttpPost("{id}/activity")]
  [SwaggerAuthorize]
  public async Task<IActionResult> Handle (Guid id, [FromBody] CreateActivityPayload payload)
  {
    await _mediator.Send(new CreateActivityCommand(id, payload, SessionFromHeaders())); 
    
    return StatusCode(201);
  }

  [HttpPut("{groupId}/activity/{activityId}")]
  [SwaggerAuthorize]
  public async Task<IActionResult> Handle (Guid groupId, Guid activityId, [FromBody] UpdateActivityPayload payload)
  {
    await _mediator.Send(new UpdateActivityCommand(
      new Aggregates(groupId, activityId),
      payload,
      SessionFromHeaders()
    ));
    
    return Ok();
  }
}