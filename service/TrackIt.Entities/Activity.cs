namespace TrackIt.Entities;

public class Activity : ActivityBase
{
  public required Guid ActivityGroupId { get; set; }
  public required List<SubActivity> SubActivities { get; set; } = [];
}