using TrackIt.Entities.Core;

namespace TrackIt.Entities.Activities;

public class ActivityGroup : Entity
{
  public required Guid UserId { get; set; }
  public required string Title { get; set; }
  public required int Order { get; set; }
  public List<Activity> Activities { get; set; } = [];

  public static ActivityGroup Create ()
  {
    return new ActivityGroup
    {
      UserId = default,
      Title = string.Empty,
      Order = 0
    };
  }

  public ActivityGroup WithTitle (string title)
  {
    Title = title;
    return this;
  }
  
  public ActivityGroup WithOrder (int order)
  {
    Order = order;
    return this;
  }
  
  public ActivityGroup AssignUser (Guid userId)
  {
    UserId = userId;
    return this;
  }
}