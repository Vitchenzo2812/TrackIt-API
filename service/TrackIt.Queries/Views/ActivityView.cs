using TrackIt.Entities;

namespace TrackIt.Queries.Views;

public record ActivityView (
  Guid Id,

  string Title,

  string? Description,

  bool Checked,

  int Order
)
{
  public static ActivityView Build (Activity activity)
  {
    return new ActivityView(
      Id: activity.Id,
      Title: activity.Title,
      Description: activity.Description,
      Checked: activity.Checked,
      Order: activity.Order
    );
  }
}