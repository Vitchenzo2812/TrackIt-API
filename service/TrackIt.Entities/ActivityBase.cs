using TrackIt.Entities.Core;

namespace TrackIt.Entities;

public class ActivityBase : Aggregate
{
  public required string Title { get; set; }
  public required string? Description { get; set; }
  public required ActivityPriority Priority { get; set; }
  public required int Order { get; set; }
  public required bool Checked { get; set; }
  public required DateTime? CompletedAt { get; set; }
  public required DateTime CreatedAt { get; set; } = DateTime.Now;
}