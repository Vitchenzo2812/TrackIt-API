using TrackIt.Entities;

namespace TrackIt.Queries.Views;

public record ActivityGroupView (
  Guid Id,

  string Icon,

  string Title,

  int Order
)
{
  public static ActivityGroupView Build (ActivityGroup activityGroup)
  {
    return new ActivityGroupView (
      Id: activityGroup.Id,
      Icon: activityGroup.Icon,
      Title: activityGroup.Title,
      Order: activityGroup.Order
    );
  }
}