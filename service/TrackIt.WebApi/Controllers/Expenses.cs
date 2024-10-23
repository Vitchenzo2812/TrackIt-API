using TrackIt.Infraestructure.Web.Swagger.Annotations;
using TrackIt.Commands.ExpenseCommands.CreateExpense;
using TrackIt.Commands.ExpenseCommands.UpdateExpense;
using TrackIt.Commands.ExpenseCommands.DeleteExpense;
using TrackIt.Infraestructure.Web.Controller;
using Microsoft.AspNetCore.Mvc;
using MediatR;

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

  [HttpPut("{id}")]
  [SwaggerAuthorize]
  public async Task<IActionResult> Handle (Guid id, [FromBody] UpdateExpensePayload payload)
  {
    await _mediator.Send(new UpdateExpenseCommand(id, payload, SessionFromHeaders()));
    
    return Ok();
  }

  [HttpDelete("{id}")]
  [SwaggerAuthorize]
  public async Task<IActionResult> Handle (Guid id)
  {
    await _mediator.Send(new DeleteExpenseCommand(id, SessionFromHeaders()));
    
    return Ok();
  }
}