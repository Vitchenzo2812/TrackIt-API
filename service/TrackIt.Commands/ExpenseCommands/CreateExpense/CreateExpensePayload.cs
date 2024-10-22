namespace TrackIt.Commands.ExpenseCommands.CreateExpense;

public record CreateExpensePayload(
  string Title,
  string? Description,
  DateTime Date,
  double Amount,
  Guid PaymentFormatId,
  Guid CategoryId,
  bool IsRecurring
);