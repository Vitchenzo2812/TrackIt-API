using TrackIt.Entities;

namespace TrackIt.Queries.Views;

public record MonthlyExpensesView (
  Guid Id,

  string? Title,

  string? Description,

  DateTime Date
)
{
  public static MonthlyExpensesView Build (MonthlyExpenses monthlyExpenses)
  {
    return new MonthlyExpensesView(
      Id: monthlyExpenses.Id,
      Title: monthlyExpenses.Title,
      Description: monthlyExpenses.Description,
      Date: monthlyExpenses.Date
    );
  }
}