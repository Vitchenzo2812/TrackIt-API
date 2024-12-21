using TrackIt.Entities.Activities;

namespace TrackIt.Queries.GetSubActivity;

public record GetSubActivityResult (
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
  public static GetSubActivityResult Build (SubActivity subActivity)
  {
    return new GetSubActivityResult(
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