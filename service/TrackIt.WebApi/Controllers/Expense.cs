using TrackIt.Commands.MonthlyExpenseCommands.CreateMonthlyExpenses;
using TrackIt.Commands.MonthlyExpenseCommands.UpdateMonthlyExpenses;
using TrackIt.Commands.MonthlyExpenseCommands.DeleteMonthlyExpenses;
using TrackIt.Infraestructure.Web.Swagger.Annotations;
using TrackIt.Infraestructure.Web.Controller;
using TrackIt.Queries.GetMonthlyExpenses;
using Microsoft.AspNetCore.Mvc;
using TrackIt.Queries.Views;
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

  [HttpGet]
  [SwaggerAuthorize]
  public async Task<ActionResult<List<MonthlyExpensesView>>> Handle ([FromQuery] GetMonthlyExpensesParams @params)
  {
    return new ActionResult<List<MonthlyExpensesView>>(
      await _mediator.Send(new GetMonthlyExpensesQuery(@params, SessionFromHeaders()))
    );
  }
  
  [HttpPost]
  [SwaggerAuthorize]
  public async Task<IActionResult> Handle ([FromBody] CreateMonthlyExpensesPayload payload)
  {
    await _mediator.Send(new CreateMonthlyExpensesCommand(payload, SessionFromHeaders()));
    
    return StatusCode(201);
  }
  
  [HttpPut("{id}")]
  [SwaggerAuthorize]
  public async Task<IActionResult> Handle (Guid id, [FromBody] UpdateMonthlyExpensesPayload payload)
  {
    await _mediator.Send(new UpdateMonthlyExpensesCommand(id, payload, SessionFromHeaders()));
    
    return Ok();
  }

  [HttpDelete("{id}")]
  [SwaggerAuthorize]
  public async Task<IActionResult> Handle (Guid id)
  {
    await _mediator.Send(new DeleteMonthlyExpensesCommand(id, SessionFromHeaders()));
    
    return Ok();
  }
}