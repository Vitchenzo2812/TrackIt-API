﻿using TrackIt.Infraestructure.Security.Models;
using TrackIt.Infraestructure.Web.Controller;
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
  public async Task<IActionResult> Handle ([FromBody] SignUpPayload payload)
  {
    await _mediator.Send(new SignUpCommand(payload));
    
    return StatusCode(201);
  }

  [HttpPost("refresh-token")]
  public async Task<ActionResult<Session>> Handle ([FromBody] RefreshTokenPayload payload)
  {
    return new ActionResult<Session>(
      await _mediator.Send(new RefreshTokenCommand(payload))
    );
  }
}