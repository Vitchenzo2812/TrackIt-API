using TrackIt.Entities.Core;

namespace TrackIt.Entities.Expenses;

public class Category : Aggregate
{
  public required string Title { get; set; }
  public required string Description { get; set; }
}