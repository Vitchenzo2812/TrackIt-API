using TrackIt.Infraestructure.Web.Controller;
using TrackIt.Commands.Auth.SignUp;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace TrackIt.WebApi.Controllers;

[Tags("Auth")]
[Route("auth")]
[ApiController]
public class Auth (IMediator mediator) : BaseController
{
  [HttpPost("sign-up")]
  public async Task<ActionResult<SignUpResponse>> Handle ([FromBody] SignUpPayload payload)
  {
    var result = await mediator.Send(new SignUpCommand(payload));
    
    return new ActionResult<SignUpResponse>(result);
  }
}