using TrackIt.Infraestructure.Web.Controller;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace TrackIt.WebApi.Controllers;

[Tags("Auth")]
[Route("auth")]
[ApiController]
public class Auth (IMediator mediator) : BaseController
{
}