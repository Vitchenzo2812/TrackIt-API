namespace TrackIt.Entities.Activities;

public class SubActivity : ActivityBase<SubActivity>
{
  public required Guid ActivityId { get; set; }

  public static SubActivity Create ()
  {
    return new SubActivity
    {
      Title = string.Empty,
      Description = null,
      Priority = ActivityPriority.LOW,
      Order = 0,
      Checked = false,
      ActivityId = default,
      CompletedAt = null
    };  
  }

  public SubActivity AssignToActivity (Guid activityId)
  {
    ActivityId = activityId;
    return this;
  }
}