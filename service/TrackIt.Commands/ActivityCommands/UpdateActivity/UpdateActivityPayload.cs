namespace TrackIt.Commands.ActivityCommands.UpdateActivity;

public record UpdateActivityAggregate (
  Guid Id,
  
  Guid EntityId
);

public record UpdateActivityPayload (
  string Title,
  
  string? Description,
  
  bool Checked,
  
  int Order
);