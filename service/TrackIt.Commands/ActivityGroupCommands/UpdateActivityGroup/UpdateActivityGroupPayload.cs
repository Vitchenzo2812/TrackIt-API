namespace TrackIt.Commands.ActivityGroupCommands.UpdateActivityGroup;

public record UpdateActivityGroupPayload (
  string Title,
  
  string Icon,
  
  int Order
);