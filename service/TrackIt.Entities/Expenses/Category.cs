using TrackIt.Entities.Core;
using TrackIt.Entities.Events;

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

  public Category SendCreateCategoryEvent (string icon, string iconColor, string backgroundIconColor)
  {
    Commit(new CreateCategoryEvent(Id, icon, iconColor, backgroundIconColor));
    return this;
  }
  
  public Category SendUpdateCategoryEvent (string icon, string iconColor, string backgroundIconColor)
  {
    Commit(new UpdateCategoryEvent(Id, icon, iconColor, backgroundIconColor));
    return this;
  }
  
  public Category SendDeleteCategoryEvent ()
  {
    Commit(new DeleteCategoryEvent(Id));
    return this;
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