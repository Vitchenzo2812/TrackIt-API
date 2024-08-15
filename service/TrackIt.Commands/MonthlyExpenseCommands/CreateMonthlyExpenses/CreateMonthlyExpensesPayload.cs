namespace TrackIt.Commands.MonthlyExpenseCommands.CreateMonthlyExpenses;

public record CreateMonthlyExpensesPayload (
  string? Title,
  
  string? Description
);