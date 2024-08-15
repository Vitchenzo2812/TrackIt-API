using TrackIt.Commands.MonthlyExpenseCommands.CreateMonthlyExpense;
using TrackIt.Infraestructure.Web.Swagger.Annotations;
using TrackIt.Infraestructure.Web.Controller;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace TrackIt.WebApi.Controllers;

[Tags("Expense")]
[Route("monthly-expense")]
[ApiController]
public class Expense : BaseController
{
  private readonly IMediator _mediator;
  
  public Expense (IMediator mediator)
  {
    _mediator = mediator;
  }

  [HttpPost]
  [SwaggerAuthorize]
  public async Task<IActionResult> Handle ([FromBody] CreateMonthlyExpensesPayload payload)
  {
    await _mediator.Send(new CreateMonthlyExpensesCommand(payload, SessionFromHeaders()));
    
    return StatusCode(201);
  }
}