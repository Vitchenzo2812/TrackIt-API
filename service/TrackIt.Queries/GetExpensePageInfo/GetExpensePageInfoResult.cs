namespace TrackIt.Queries.GetExpensePageInfo;

public record GetExpensePageInfoResult (
  Guid MonthlyExpenseId,
  string Title,
  int TotalExpenses,
  double PercentageLimitExpenses,
  int? LimitExpenses
)
{
  public static GetExpensePageInfoResult Build (MonthlyExpenseRow row)
  {
    var percentageLimitExpenses = 0.0;

    if (row.LimitExpenses is not null && row.LimitExpenses != 0)
      percentageLimitExpenses = ((row.TotalExpenses / (double)row.LimitExpenses) * 100);
    
    return new GetExpensePageInfoResult(
      MonthlyExpenseId: row.Id,
      Title: row.Title,
      TotalExpenses: row.TotalExpenses,
      LimitExpenses: row.LimitExpenses,
      PercentageLimitExpenses: percentageLimitExpenses
    );
  }
}