using TrackIt.Commands.ActivityCommands.CreateActivity;
using TrackIt.Commands.ActivityCommands.DeleteActivity;
using TrackIt.Commands.ActivityCommands.UpdateActivity;
using TrackIt.Infraestructure.Web.Swagger.Annotations;
using TrackIt.Infraestructure.Web.Controller;
using TrackIt.Commands.ActivityCommands;
using TrackIt.Queries.GetActivities;
using TrackIt.Queries.GetActivity;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace TrackIt.WebApi.Controllers;

[Tags("Activity")]
[Route("group")]
[ApiController]
public class Activity : BaseController
{
  private readonly IMediator _mediator;

  public Activity (IMediator mediator) => _mediator = mediator;
  
  [HttpGet("{id}/activity")]
  [SwaggerAuthorize]
  public async Task<List<GetActivitiesResult>> HandleGetActivities (Guid id)
  {
    return await _mediator.Send(new GetActivitiesQuery(new GetActivitiesParams(id), SessionFromHeaders()));
  }
  
  [HttpGet("{id}/activity/{activityId}")]
  [SwaggerAuthorize]
  public async Task<GetActivityResult> HandleGetActivity (Guid id, Guid activityId)
  {
    return await _mediator.Send(
      new GetActivityQuery(new GetActivityParams(id, activityId), SessionFromHeaders())
    );
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
}