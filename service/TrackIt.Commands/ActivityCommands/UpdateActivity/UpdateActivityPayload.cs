namespace TrackIt.Commands.ActivityCommands.UpdateActivity;

public record UpdateActivityPayload (
  Guid ActivityId,
  
  string Title,
  
  string Description,
  
  bool Checked,
  
  int Order
);