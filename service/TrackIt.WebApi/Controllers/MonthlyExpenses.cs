using TrackIt.Commands.MonthlyExpensesCommands.UpdateMonthlyExpenses;
using TrackIt.Commands.MonthlyExpensesCommands.DeleteMonthlyExpenses;
using TrackIt.Infraestructure.Web.Swagger.Annotations;
using TrackIt.Infraestructure.Web.Controller;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using TrackIt.Queries.GetExpensePageInfo;

namespace TrackIt.WebApi.Controllers;

[Tags("Monthly Expense")]
[Route("monthly-expenses")]
[ApiController]
public class MonthlyExpenses : BaseController
{
  private readonly IMediator _mediator;

  public MonthlyExpenses (IMediator mediator) => _mediator = mediator;

  [HttpGet]
  [SwaggerAuthorize]
  public async Task<List<GetExpensePageInfoResult>> Handle ([FromQuery] GetExpensePageInfoParams @params)
  {
    return await _mediator.Send(new GetExpensePageInfoQuery(@params, SessionFromHeaders()));
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