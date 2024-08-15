namespace TrackIt.Commands.MonthlyExpenseCommands.CreateMonthlyExpense;

public record CreateMonthlyExpensesPayload (
  string? Title,
  
  string? Description
);