namespace TrackIt.Commands.MonthlyExpenseCommands.UpdateMonthlyExpenses;

public record UpdateMonthlyExpensesPayload (
  string? Title,
  
  string? Description
);