using TrackIt.Commands.SubActivityCommands.CreateSubActivity;
using TrackIt.Commands.SubActivityCommands.DeleteSubActivity;
using TrackIt.Commands.SubActivityCommands.UpdateSubActivity;
using TrackIt.Infraestructure.Web.Swagger.Annotations;
using TrackIt.Infraestructure.Web.Controller;
using TrackIt.Commands.SubActivityCommands;
using TrackIt.Queries.GetSubActivities;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace TrackIt.WebApi.Controllers;

[Tags("Sub Activity")]
[Route("group")]
[ApiController]
public class SubActivity : BaseController
{
  private readonly IMediator _mediator;

  public SubActivity (IMediator mediator) => _mediator = mediator;
  
  [HttpGet("{groupId}/activity/{activityId}/sub")]
  [SwaggerAuthorize]
  public async Task<List<GetSubActivitiesResult>> HandleGetSubActivities (Guid groupId, Guid activityId)
  {
    return await _mediator.Send(
      new GetSubActivitiesQuery(new GetSubActivitiesParams(groupId, activityId), SessionFromHeaders())
    );
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