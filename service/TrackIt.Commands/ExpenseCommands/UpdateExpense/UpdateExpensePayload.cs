namespace TrackIt.Commands.ExpenseCommands.UpdateExpense;

public record UpdateExpensePayload(
  string Title,
  string? Description,
  DateTime Date,
  double Amount,
  Guid PaymentFormatId,
  Guid CategoryId,
  bool IsRecurring
);