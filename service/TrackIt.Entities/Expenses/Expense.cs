using TrackIt.Entities.Core;

namespace TrackIt.Entities.Expenses;

public class Expense : Aggregate
{
  public required string Title { get; set; }
  public required string? Description { get; set; }
  public required DateTime Date { get; set; }
  public required double Amount { get; set; }
  public required bool IsRecurring { get; set; }
  public required Guid CategoryId { get; set; }
  public required Guid MonthlyExpensesId { get; set; }
  public required Guid PaymentFormatId { get; set; }
}