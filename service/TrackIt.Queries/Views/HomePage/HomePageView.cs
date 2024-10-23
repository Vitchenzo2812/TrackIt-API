namespace TrackIt.Queries.Views.HomePage;

public record HomePageView (
  int PercentageCompletedActivities,
  PaginationView<List<PartialActivityView>> CompletedActivities,
  PaginationView<List<PartialActivityView>> IncompleteActivities
)
{
  public static HomePageView Build (
    int percentageCompletedActivities,
    PaginationView<List<PartialActivityView>> completedActivities,
    PaginationView<List<PartialActivityView>> incompleteActivities
  )
  {
    return new HomePageView(
      PercentageCompletedActivities: percentageCompletedActivities,
      CompletedActivities: completedActivities,
      IncompleteActivities: incompleteActivities
    );
  }
}