using TrackIt.Commands.ExpenseCommands.CreateExpense;
using TrackIt.Infraestructure.Web.Controller;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using TrackIt.Infraestructure.Web.Swagger.Annotations;

namespace TrackIt.WebApi.Controllers;

[Tags("Expense")]
[Route("expense")]
[ApiController]
public class Expenses : BaseController
{
  private readonly IMediator _mediator;

  public Expenses (IMediator mediator) => _mediator = mediator;
  
  [HttpPost]
  [SwaggerAuthorize]
  public async Task<IActionResult> Handle ([FromBody] CreateExpensePayload payload)
  {
    await _mediator.Send(new CreateExpenseCommand(payload, SessionFromHeaders()));
    
    return StatusCode(201);
  }
}