using TrackIt.Commands.ActivityGroupCommands.CreateActivityGroup;
using TrackIt.Commands.ActivityGroupCommands.UpdateActivityGroup;
using TrackIt.Commands.ActivityGroupCommands.DeleteActivityGroup;
using TrackIt.Commands.SubActivityCommands.CreateSubActivity;
using TrackIt.Commands.SubActivityCommands.UpdateSubActivity;
using TrackIt.Commands.SubActivityCommands.DeleteSubActivity;
using TrackIt.Commands.ActivityCommands.CreateActivity;
using TrackIt.Commands.ActivityCommands.UpdateActivity;
using TrackIt.Commands.ActivityCommands.DeleteActivity;
using TrackIt.Infraestructure.Web.Swagger.Annotations;
using TrackIt.Infraestructure.Web.Controller;
using TrackIt.Commands.SubActivityCommands;
using TrackIt.Commands.ActivityCommands;
using TrackIt.Queries.GetHomePageInfo;
using TrackIt.Queries.Views.HomePage;
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

  [HttpGet("home-page")]
  [SwaggerAuthorize]
  public async Task<HomePageView> Handle ([FromQuery] GetHomePageInfoParams @params)
  {
    return await _mediator.Send(new GetHomePageInfoQuery(@params, SessionFromHeaders()));
  }
  
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
      new ActivityAggregates(groupId, activityId),
      payload,
      SessionFromHeaders()
    ));
    
    return Ok();
  }

  [HttpDelete("{groupId}/activity/{activityId}")]
  [SwaggerAuthorize]
  public async Task<IActionResult> Handle (Guid groupId, Guid activityId)
  {
    await _mediator.Send(new DeleteActivityCommand(new ActivityAggregates(groupId, activityId), SessionFromHeaders()));
    
    return Ok();
  }

  [HttpPost("{groupId}/activity/{activityId}/sub")]
  [SwaggerAuthorize]
  public async Task<IActionResult> Handle (Guid groupId, Guid activityId, [FromBody] CreateSubActivityPayload payload)
  {
    await _mediator.Send(new CreateSubActivityCommand(
      new SubActivityAggregates(groupId, activityId),
      payload,
      SessionFromHeaders()
    ));
    
    return StatusCode(201);
  }

  [HttpPut("{groupId}/activity/{activityId}/sub/{subId}")]
  [SwaggerAuthorize]
  public async Task<IActionResult> Handle (Guid groupId, Guid activityId, Guid subId, [FromBody] UpdateSubActivityPayload payload)
  {
    await _mediator.Send(new UpdateSubActivityCommand(
      new SubActivityAggregates(groupId, activityId, subId),
      payload,
      SessionFromHeaders()
    ));
    
    return Ok();
  }

  [HttpDelete("{groupId}/activity/{activityId}/sub/{subId}")]
  [SwaggerAuthorize]
  public async Task<IActionResult> Handle (Guid groupId, Guid activityId, Guid subId)
  {
    await _mediator.Send(new DeleteSubActivityCommand(
      new SubActivityAggregates(groupId, activityId, subId), 
      SessionFromHeaders()
    ));
    
    return Ok();
  }
}