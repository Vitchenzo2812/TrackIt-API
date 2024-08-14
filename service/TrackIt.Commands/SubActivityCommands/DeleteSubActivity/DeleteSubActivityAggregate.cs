namespace TrackIt.Commands.SubActivityCommands.DeleteSubActivity;

public record DeleteSubActivityAggregate (
  Guid ActivityGroupId,
  
  Guid ActivityId,
  
  Guid SubActivityId
);