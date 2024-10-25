using TrackIt.Entities.Activities;

namespace TrackIt.Queries.GetActivities;

public record GetActivitiesResult (
  Guid Id,
  string Title,
  string? Description,
  ActivityPriority Priority,
  int Order,
  bool Checked,
  DateTime? CompletedAt
)
{
  public static GetActivitiesResult Build (Activity activity)
  {
    return new GetActivitiesResult(
      Id: activity.Id,
      Title: activity.Title,
      Description: activity.Description,
      Priority: activity.Priority,
      Order: activity.Order,
      Checked: activity.Checked,
      CompletedAt: activity.CompletedAt
    );
  }
}