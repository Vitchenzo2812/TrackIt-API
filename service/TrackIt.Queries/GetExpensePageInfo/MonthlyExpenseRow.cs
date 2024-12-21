namespace TrackIt.Queries.GetExpensePageInfo;

public record MonthlyExpenseRow (
  Guid Id,
  string Title,
  int TotalExpenses,
  int? LimitExpenses
);