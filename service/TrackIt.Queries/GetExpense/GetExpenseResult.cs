using TrackIt.Entities.Expenses;

namespace TrackIt.Queries.GetExpense;

public record GetExpenseResult (
  Guid Id,
  string Title,
  string? Description,
  DateTime Date,
  double Amount,
  bool IsRecurring,
  Guid CategoryId,
  Guid PaymentFormatId
)
{
  public static GetExpenseResult Build (Expense expense)
  {
    return new GetExpenseResult(
      Id: expense.Id,
      Title: expense.Title,
      Description: expense.Description,
      Date: expense.Date,
      Amount: expense.Amount,
      IsRecurring: expense.IsRecurring,
      CategoryId: expense.CategoryId,
      PaymentFormatId: expense.PaymentFormatId
    );
  }
}