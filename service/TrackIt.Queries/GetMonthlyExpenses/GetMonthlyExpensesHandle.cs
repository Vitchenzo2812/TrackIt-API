using TrackIt.Infraestructure.Database;
using Microsoft.EntityFrameworkCore;
using TrackIt.Queries.Views;
using MediatR;

namespace TrackIt.Queries.GetMonthlyExpenses;

public class GetMonthlyExpensesHandle : IRequestHandler<GetMonthlyExpensesQuery, List<MonthlyExpensesView>>
{
  private readonly TrackItDbContext _db;

  public GetMonthlyExpensesHandle (TrackItDbContext db)
  {
    _db = db;
  }
  
  public async Task<List<MonthlyExpensesView>> Handle (GetMonthlyExpensesQuery request, CancellationToken cancellationToken)
  {
    var monthlyExpenses = await _db.MonthlyExpenses
      .Where(m => m.UserId == request.Params.UserId)
      .ToListAsync();

    return monthlyExpenses.Select(MonthlyExpensesView.Build).ToList();
  }
}