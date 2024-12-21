namespace TrackIt.Queries.GetSubActivity;

public record GetSubActivityParams (
  Guid ActivityGroupId,
  Guid ActivityId,
  Guid SubActivityId
);