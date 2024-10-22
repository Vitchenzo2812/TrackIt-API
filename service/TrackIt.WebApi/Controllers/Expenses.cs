using TrackIt.Commands.ExpenseCommands.CreateExpense;
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
  public async Task<IActionResult> Handle ([FromBody] CreateExpensePayload payload)
  {
    await _mediator.Send(new CreateExpenseCommand(payload, SessionFromHeaders()));
    
    return StatusCode(201);
  }
}