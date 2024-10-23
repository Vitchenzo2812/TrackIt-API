namespace TrackIt.Commands.MonthlyExpensesCommands.UpdateMonthlyExpenses;

public record UpdateMonthlyExpensesPayload(
  string Title,
  int? Limit
);