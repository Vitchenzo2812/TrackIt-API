using TrackIt.Entities.Core;

namespace TrackIt.Entities;

public class ActivityGroup : Entity
{
  public Guid UserId { get; set; }

  public string Icon { get; set; } = string.Empty;
  
  public string Title { get; set; } = string.Empty;

  public int Order { get; set; }
  
  public List<Activity> Activities { get; set; } = [];

  public static ActivityGroup Create (
    Guid userId,
    string title,
    string icon,
    int order
  )
  {
    return new ActivityGroup
    {
      UserId = userId,
      
      Title = title,
      
      Icon = icon,
      
      Order = order
    };
  }

  public void Update (
    string title,
    string icon,
    int order
  )
  {
    Title = title;
    Icon = icon;
    Order = order;
  }
}