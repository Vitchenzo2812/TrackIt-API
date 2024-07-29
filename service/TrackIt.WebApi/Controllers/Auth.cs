using TrackIt.Commands.Auth.VerifyForgotPassword;
using TrackIt.Infraestructure.Security.Models;
using TrackIt.Infraestructure.Web.Controller;
using TrackIt.Commands.Auth.EmailValidation;
using TrackIt.Commands.Auth.ForgotPassword;
using TrackIt.Commands.Auth.RefreshToken;
using TrackIt.Commands.Auth.SignIn;
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

  [HttpPost("sign-in")]
  public async Task<ActionResult<Session>> Handle ([FromBody] SignInPayload payload)
  {
    return new ActionResult<Session>(
      await _mediator.Send(new SignInCommand(payload))
    );
  }
  
  [HttpPost("sign-up")]
  public async Task<ActionResult<SignUpResponse>> Handle ([FromBody] SignUpPayload payload)
  {
    return new ActionResult<SignUpResponse>(
      await _mediator.Send(new SignUpCommand(payload))
    );
  }

  [HttpPost("email-verification")]
  public async Task<ActionResult<Session>> Handle ([FromBody] EmailValidationPayload payload)
  {
    return new ActionResult<Session>(
      await _mediator.Send(new EmailValidationCommand(payload))
    );
  }

  [HttpPost("forgot-password")]
  public async Task<ActionResult<ForgotPasswordResponse>> Handle ([FromBody] ForgotPasswordPayload payload)
  {
    return new ActionResult<ForgotPasswordResponse>(
        await _mediator.Send(new ForgotPasswordCommand(payload))
      );
  }

  [HttpPost("forgot-password/verify")]
  public async Task<IActionResult> Handle ([FromBody] VerifyForgotPasswordPayload payload)
  {
    await _mediator.Send(new VerifyForgotPasswordCommand(payload));

    return Ok();
  }
  
  [HttpPost("refresh-token")]
  public async Task<ActionResult<Session>> Handle ([FromBody] RefreshTokenPayload payload)
  {
    return new ActionResult<Session>(
      await _mediator.Send(new RefreshTokenCommand(payload))
    );
  }
}