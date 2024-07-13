using TrackIt.Infraestructure.Web.Swagger.Annotations;
using TrackIt.Infraestructure.Web.Controller;
using TrackIt.Commands.UpdateUser;
using Microsoft.AspNetCore.Mvc;
using TrackIt.Queries.GetUser;
using TrackIt.Queries.Views;
using MediatR;

namespace TrackIt.WebApi.Controllers;

[Tags("User")]
[Route("user")]
[ApiController]
public class User : BaseController
{
  private readonly IMediator _mediator;

  public User (IMediator mediator)
  {
    _mediator = mediator;
  }

  [HttpGet("{id}")]
  [SwaggerAuthorize]
  public async Task<ActionResult<UserView>> Handle (Guid id)
  {
    return new ActionResult<UserView>(
      await _mediator.Send(new GetUserQuery(new GetUserParams(id), SessionFromHeaders()))
    );
  }

  [HttpPut("{id}")]
  [SwaggerAuthorize]
  public async Task<IActionResult> Handle (Guid id, [FromBody] UpdateUserPayload payload)
  {
    await _mediator.Send(new UpdateUserCommand(id, payload));

    return Ok();
  }
}