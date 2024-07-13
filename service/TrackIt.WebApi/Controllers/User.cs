using TrackIt.Infraestructure.Web.Swagger.Annotations;
using TrackIt.Infraestructure.Web.Controller;
using TrackIt.Commands.UpdateUser;
using Microsoft.AspNetCore.Mvc;
using TrackIt.Queries.GetUser;
using TrackIt.Queries.Views;
using MediatR;
using TrackIt.Commands.DeleteUser;

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
  public async Task<ActionResult<UserView>> HandleGetUser (Guid id)
  {
    return new ActionResult<UserView>(
      await _mediator.Send(new GetUserQuery(new GetUserParams(id), SessionFromHeaders()))
    );
  }

  [HttpPut("{id}")]
  [SwaggerAuthorize]
  public async Task<IActionResult> HandleUpdateUser (Guid id, [FromBody] UpdateUserPayload payload)
  {
    await _mediator.Send(new UpdateUserCommand(id, payload, SessionFromHeaders()));

    return Ok();
  }

  [HttpDelete("{id}")]
  [SwaggerAuthorize]
  public async Task<IActionResult> HandleDeleteUser (Guid id)
  {
    await _mediator.Send(new DeleteUserCommand(id, SessionFromHeaders()));
    
    return Ok();
  }
}