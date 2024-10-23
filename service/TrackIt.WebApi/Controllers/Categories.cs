using TrackIt.Commands.CategoryCommands.CreateCategory;
using TrackIt.Infraestructure.Web.Swagger.Annotations;
using TrackIt.Infraestructure.Web.Controller;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace TrackIt.WebApi.Controllers;

[Tags("Category")]
[Route("category")]
[ApiController]
public class Categories : BaseController
{
  private readonly IMediator _mediator;

  public Categories (IMediator mediator) => _mediator = mediator;
  
  [HttpPost]
  [SwaggerAuthorize]
  public async Task<IActionResult> Handle ([FromBody] CreateCategoryPayload payload)
  {
    await _mediator.Send(new CreateCategoryCommand(payload, SessionFromHeaders()));
    
    return StatusCode(201);
  }
}