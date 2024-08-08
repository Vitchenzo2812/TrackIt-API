using TrackIt.Entities.Core;

namespace TrackIt.Entities;

public class ActivityGroup : Entity
{
  public Guid UserId { get; set; }

  public string Icon { get; set; } = string.Empty;
  
  public string Title { get; set; } = string.Empty;

  public int Order { get; set; }
  
  public List<Activity> Activities { get; set; } = [];
}