using TrackIt.Commands.ActivityGroupCommands.CreateActivityGroup;
using TrackIt.Commands.ActivityGroupCommands.UpdateActivityGroup;
using TrackIt.Commands.ActivityGroupCommands.DeleteActivityGroup;
using TrackIt.Commands.ActivityCommands.DeleteActivity;
using TrackIt.Commands.ActivityCommands.UpdateActivity;
using TrackIt.Commands.ActivityCommands.CreateActivity;
using TrackIt.Infraestructure.Web.Swagger.Annotations;
using TrackIt.Infraestructure.Web.Controller;
using TrackIt.Queries.GetActivitiesGroups;
using Microsoft.AspNetCore.Mvc;
using TrackIt.Queries.Views;
using MediatR;

namespace TrackIt.WebApi.Controllers;

[Tags("Activity")]
[Route("activity-group")]
[ApiController]
public class Activity : BaseController
{
  private readonly IMediator _mediator;
  
  public Activity (IMediator mediator)
  {
    _mediator = mediator;
  }

  [HttpGet]
  [SwaggerAuthorize]
  public async Task<ActionResult<PaginationView<List<ActivityGroupView>>>> Handle ([FromQuery] GetActivitiesGroupsParams @params)
  {
    return new ActionResult<PaginationView<List<ActivityGroupView>>>(
      await _mediator.Send(new GetActivitiesGroupsQuery(@params, SessionFromHeaders()))
    );
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
  
  [HttpPut("{id}/activity/{activityId}")]
  [SwaggerAuthorize]
  public async Task<IActionResult> Handle (Guid id, Guid activityId, [FromBody] UpdateActivityPayload payload)
  {
    await _mediator.Send(
      new UpdateActivityCommand(
        new UpdateActivityAggregate(id, activityId), 
        payload, 
        SessionFromHeaders()
      )
    );
    
    return Ok();
  }

  [HttpDelete("{id}/activity/{activityId}")]
  [SwaggerAuthorize]
  public async Task<IActionResult> Handle (Guid id, Guid activityId)
  {
    await _mediator.Send(
      new DeleteActivityCommand(
        new DeleteActivityAggreagate(id, activityId), 
        SessionFromHeaders()
      )
    );

    return Ok();
  }
}