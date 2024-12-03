using TrackIt.Entities.Activities;

namespace TrackIt.Queries.GetSubActivities;

public record GetSubActivitiesResult (
  Guid Id,
  Guid ActivityId,
  string Title,
  string? Description,
  ActivityPriority Priority,
  int Order,
  bool Checked,
  DateTime? CompletedAt
)
{
  public static GetSubActivitiesResult Build (SubActivity subActivity)
  {
    return new GetSubActivitiesResult(
      Id: subActivity.Id,
      ActivityId: subActivity.ActivityId,
      Title: subActivity.Title,
      Description: subActivity.Description,
      Priority: subActivity.Priority,
      Order: subActivity.Order,
      Checked: subActivity.Checked,
      CompletedAt: subActivity.CompletedAt
    );
  }
}