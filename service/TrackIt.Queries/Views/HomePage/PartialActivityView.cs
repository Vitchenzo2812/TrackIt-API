using TrackIt.Entities.Activities;

namespace TrackIt.Queries.Views.HomePage;

public record PartialActivityView (
  string Title,
  string? Description
)
{
  public static PartialActivityView Build (Activity activity)
  {
    return new PartialActivityView(
      Title: activity.Title,
      Description: activity.Description
    );
  }
}