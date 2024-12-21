using TrackIt.Commands.CategoryCommands.CreateCategory;
using TrackIt.Commands.CategoryCommands.UpdateCategory;
using TrackIt.Commands.CategoryCommands.DeleteCategory;
using TrackIt.Infraestructure.Web.Swagger.Annotations;
using TrackIt.Infraestructure.Web.Controller;
using TrackIt.Queries.GetCategories;
using TrackIt.Queries.GetCategory;
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

  [HttpGet]
  [SwaggerAuthorize]
  public async Task<List<GetCategoriesResult>> Handle ()
  {
    return await _mediator.Send(new GetCategoriesQuery(SessionFromHeaders()));
  }

  [HttpGet("{id}")]
  [SwaggerAuthorize]
  public async Task<GetCategoryResult> HandleGetCategory (Guid id)
  {
    return await _mediator.Send(
      new GetCategoryQuery(new GetCategoryParams(id), SessionFromHeaders())
    );
  }
  
  [HttpPost]
  [SwaggerAuthorize]
  public async Task<IActionResult> Handle ([FromBody] CreateCategoryPayload payload)
  {
    await _mediator.Send(new CreateCategoryCommand(payload, SessionFromHeaders()));
    
    return StatusCode(201);
  }

  [HttpPut("{id}")]
  [SwaggerAuthorize]
  public async Task<IActionResult> Handle (Guid id, [FromBody] UpdateCategoryPayload payload)
  {
    await _mediator.Send(new UpdateCategoryCommand(id, payload, SessionFromHeaders()));
    
    return Ok();
  }

  [HttpDelete("{id}")]
  [SwaggerAuthorize]
  public async Task<IActionResult> Handle (Guid id)
  {
    await _mediator.Send(new DeleteCategoryCommand(id, SessionFromHeaders()));
    
    return Ok();
  }
}