using TrackIt.Entities.Core;

namespace TrackIt.Entities.Expenses;

public class Category : Aggregate
{
  public required string Title { get; set; }
  public required string Description { get; set; }
  
  public CategoryConfig? CategoryConfig { get; set; }

  public static Category Create ()
  {
    return new Category
    {
      Title = string.Empty,
      Description = string.Empty
    };
  }

  public Category WithTitle (string title)
  {
    Title = title;
    return this;
  }
  
  public Category WithDescription (string description)
  {
    Description = description;
    return this;
  }
}