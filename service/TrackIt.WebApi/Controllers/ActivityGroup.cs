using TrackIt.Commands.ActivityGroupCommands.CreateActivityGroup;
using TrackIt.Commands.ActivityGroupCommands.DeleteActivityGroup;
using TrackIt.Commands.ActivityGroupCommands.UpdateActivityGroup;
using TrackIt.Infraestructure.Web.Swagger.Annotations;
using TrackIt.Infraestructure.Web.Controller;
using TrackIt.Queries.GetActivityGroups;
using TrackIt.Queries.GetHomePageInfo;
using TrackIt.Queries.Views.HomePage;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using TrackIt.Queries.GetActivityGroup;

namespace TrackIt.WebApi.Controllers;

[Tags("Activity Group")]
[Route("group")]
[ApiController]
public class ActivityGroup : BaseController
{
  private readonly IMediator _mediator;

  public ActivityGroup (IMediator mediator) => _mediator = mediator;
  
  [HttpGet("home-page")]
  [SwaggerAuthorize]
  public async Task<HomePageView> Handle ([FromQuery] GetHomePageInfoParams @params)
  {
    return await _mediator.Send(new GetHomePageInfoQuery(@params, SessionFromHeaders()));
  }

  [HttpGet("{id}")]
  [SwaggerAuthorize]
  public async Task<GetActivityGroupResult> HandleGetActivityGroup (Guid id)
  {
    return await _mediator.Send(new GetActivityGroupQuery(new GetActivityGroupParams(id), SessionFromHeaders()));
  }
  
  [HttpGet]
  [SwaggerAuthorize]
  public async Task<List<GetActivityGroupsResult>> Handle ()
  {
    return await _mediator.Send(new GetActivityGroupsQuery(SessionFromHeaders()));
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
}