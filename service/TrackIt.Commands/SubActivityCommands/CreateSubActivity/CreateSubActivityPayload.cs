namespace TrackIt.Commands.SubActivityCommands.CreateSubActivity;

public record CreateSubActivityAggregate (
  Guid ActivityGroupId,
  
  Guid ActivityId
);

public record CreateSubActivityPayload (
  string Title,
  
  string? Description,
  
  bool Checked
);