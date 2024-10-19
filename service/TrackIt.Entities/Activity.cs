namespace TrackIt.Entities;

public class Activity : ActivityBase<Activity>
{
  public required Guid ActivityGroupId { get; set; }
  public List<SubActivity> SubActivities { get; set; } = [];

  public static Activity Create ()
  {
    return new Activity
    {
      Title = string.Empty,
      Description = null,
      Priority = ActivityPriority.LOW,
      Order = 0,
      Checked = false,
      CompletedAt = null,
      ActivityGroupId = default,
    };
  }

  public Activity AssignToGroup (Guid activityGroupId)
  {
    ActivityGroupId = activityGroupId;
    return this;
  }
}