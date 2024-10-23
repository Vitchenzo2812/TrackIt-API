using TrackIt.Commands.MonthlyExpensesCommands.UpdateMonthlyExpenses;
using TrackIt.Infraestructure.Web.Swagger.Annotations;
using TrackIt.Infraestructure.Web.Controller;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace TrackIt.WebApi.Controllers;

[Tags("Monthly Expense")]
[Route("monthly-expenses")]
[ApiController]
public class MonthlyExpenses : BaseController
{
  private readonly IMediator _mediator;

  public MonthlyExpenses (IMediator mediator) => _mediator = mediator;

  [HttpPut("{id}")]
  [SwaggerAuthorize]
  public async Task<IActionResult> Handle (Guid id, [FromBody] UpdateMonthlyExpensesPayload payload)
  {
    await _mediator.Send(new UpdateMonthlyExpensesCommand(id, payload, SessionFromHeaders()));
    
    return Ok();
  }
}