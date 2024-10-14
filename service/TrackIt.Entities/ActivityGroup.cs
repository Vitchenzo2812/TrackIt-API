using TrackIt.Entities.Core;

namespace TrackIt.Entities;

public class ActivityGroup : Entity
{
  public required Guid UserId { get; set; }
  public required string Title { get; set; }
  public required int Order { get; set; }
  public required List<Activity> Activities { get; set; } = [];
}