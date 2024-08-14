using TrackIt.Entities.Core;

namespace TrackIt.Entities;

public class MonthlyExpenses : Aggregate
{
  public Guid UserId { get; set; }
  
  public string? Title { get; set; }
  
  public string? Description { get; set; }
  
  public DateTime Date { get; set; } = DateTime.Now;

  public List<Expense> Expenses { get; set; } = [];
}