namespace TrackIt.Queries.GetActivity;

public record GetActivityParams (
  Guid ActivityGroupId,
  
  Guid ActivityId
);