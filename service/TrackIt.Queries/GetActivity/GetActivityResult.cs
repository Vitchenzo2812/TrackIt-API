using TrackIt.Entities.Activities;

namespace TrackIt.Queries.GetActivity;

public record GetActivityResult (
  Guid Id,
  Guid ActivityGroupId,
  string Title,
  string? Description,
  ActivityPriority Priority,
  int Order,
  bool Checked,
  DateTime? CompletedAt
)
{
  public static GetActivityResult Build (Activity activity)
  {
    return new GetActivityResult(
      Id: activity.Id,
      ActivityGroupId: activity.ActivityGroupId,
      Title: activity.Title,
      Description: activity.Description,
      Priority: activity.Priority,
      Order: activity.Order,
      Checked: activity.Checked,
      CompletedAt: activity.CompletedAt
    );
  }
}