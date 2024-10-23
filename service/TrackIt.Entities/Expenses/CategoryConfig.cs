using TrackIt.Entities.Core;

namespace TrackIt.Entities.Expenses;

public class CategoryConfig : Entity
{
  public Guid? CategoryId { get; set; }
  public Category? Category { get; set; }
  
  public required string Icon { get; set; }
  public required string IconColor { get; set; }
  public required string BackgroundIconColor { get; set; }

  public static CategoryConfig Create ()
  {
    return new CategoryConfig
    {
      Icon = string.Empty,
      IconColor = string.Empty,
      BackgroundIconColor = string.Empty
    };
  }

  public CategoryConfig WithIcon (string icon)
  {
    Icon = icon;
    return this;
  }
  
  public CategoryConfig WithIconColor (string iconColor)
  {
    IconColor = iconColor;
    return this;
  }
  
  public CategoryConfig WithBackgroundIconColor (string backgroundIconColor)
  {
    BackgroundIconColor = backgroundIconColor;
    return this;
  }
  
  public CategoryConfig AssignToCategory (Guid categoryId)
  {
    CategoryId = categoryId;
    return this;
  }
}