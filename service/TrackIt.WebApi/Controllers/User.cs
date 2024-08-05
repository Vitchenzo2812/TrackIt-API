using TrackIt.Infraestructure.Web.Swagger.Annotations;
using TrackIt.Infraestructure.Web.Controller;
using TrackIt.Commands.UpdateUser;
using TrackIt.Commands.DeleteUser;
using TrackIt.Queries.GetUsers;
using Microsoft.AspNetCore.Mvc;
using TrackIt.Queries.GetUser;
using TrackIt.Queries.Views;
using MediatR;
using TrackIt.Commands.UpdatePassword;

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

  [HttpGet]
  [SwaggerAuthorize]
  public async Task<ActionResult<PaginationView<List<UserResourceView>>>> HandleGetUsers ([FromQuery] GetUsersParams @params)
  {
    return new ActionResult<PaginationView<List<UserResourceView>>>(
      await _mediator.Send(new GetUsersQuery(@params, SessionFromHeaders()))
    );
  }
  
  [HttpPut("{id}")]
  [SwaggerAuthorize]
  public async Task<IActionResult> HandleUpdateUser (Guid id, [FromBody] UpdateUserPayload payload)
  {
    await _mediator.Send(new UpdateUserCommand(id, payload, SessionFromHeaders()));

    return Ok();
  }

  [HttpPut("password")]
  [SwaggerAuthorize]
  public async Task<IActionResult> HandleUpdatePassword ([FromBody] UpdatePasswordPayload payload)
  {
    await _mediator.Send(new UpdatePasswordCommand(payload, SessionFromHeaders()));

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