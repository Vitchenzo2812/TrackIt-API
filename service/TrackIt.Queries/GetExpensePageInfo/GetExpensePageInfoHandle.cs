using TrackIt.Infraestructure.Database;
using Microsoft.EntityFrameworkCore;
using TrackIt.Entities.Errors;
using MediatR;

namespace TrackIt.Queries.GetExpensePageInfo;

public class GetExpensePageInfoHandle : IRequestHandler<GetExpensePageInfoQuery, List<GetExpensePageInfoResult>>
{
  private readonly TrackItDbContext _db;

  public GetExpensePageInfoHandle (TrackItDbContext db) => _db = db;
  
  public async Task<List<GetExpensePageInfoResult>> Handle (GetExpensePageInfoQuery request, CancellationToken cancellationToken)
  {
    var sql = await _db.Database
      .SqlQueryRaw<MonthlyExpenseRow>(
        $"""
            SELECT
              monthlyExpense.Id AS Id,
              monthlyExpense.Title AS Title,
              monthlyExpense.Limit AS LimitExpenses,
              COUNT(expense.Id) AS TotalExpenses
            FROM MonthlyExpenses monthlyExpense
            LEFT JOIN Expenses expense ON expense.MonthlyExpensesId = monthlyExpense.Id
            GROUP BY monthlyExpense.Id
         """
      ).ToListAsync();

    if (sql is null)
      throw new NotFoundError("Monthly Expenses not found");

    return sql.Select(GetExpensePageInfoResult.Build).ToList();
  }
}