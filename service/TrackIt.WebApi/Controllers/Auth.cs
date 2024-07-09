using TrackIt.Infraestructure.Web.Controller;
using TrackIt.Commands.Auth.SignUp;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace TrackIt.WebApi.Controllers;

[Tags("Auth")]
[Route("auth")]
[ApiController]
public class Auth : BaseController
{
  private IMediator _mediator;
  
  public Auth (IMediator mediator)
  {
    _mediator = mediator;
  }
  
  [HttpPost("sign-up")]
  public async Task<ActionResult<SignUpResponse>> Handle ([FromBody] SignUpPayload payload)
  {
    return new ActionResult<SignUpResponse>(
      await _mediator.Send(new SignUpCommand(payload))
    );
  }
}