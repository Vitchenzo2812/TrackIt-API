using TrackIt.Entities.Core;

namespace TrackIt.Entities.Expenses;

public class MonthlyExpenses : Entity
{
  public required Guid UserId { get; set; }
  public required string Title { get; set; }
  public required DateTime Date { get; set; }
  public required int? Limit { get; set; }
}