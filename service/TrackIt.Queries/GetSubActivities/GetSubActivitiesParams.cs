namespace TrackIt.Queries.GetSubActivities;

public record GetSubActivitiesParams (
  Guid ActivityGroupId,
  Guid ActivityId
);