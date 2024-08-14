namespace TrackIt.Commands.SubActivityCommands.UpdateSubActivity;

public record UpdateSubActivityAggregate (
  Guid ActivityGroupId,
  
  Guid ActivityId,
  
  Guid SubActivityId
);


public record UpdateSubActivityPayload (
  string Title,
  
  string? Description,
  
  bool Checked,
  
  int Order
);