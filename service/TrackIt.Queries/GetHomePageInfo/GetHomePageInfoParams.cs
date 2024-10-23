namespace TrackIt.Queries.GetHomePageInfo;

public record GetHomePageInfoParams(
  Guid ActivityGroupId,
  int CompletedActivitiesPerPage,
  int IncompletedActivitiesPerPage
);