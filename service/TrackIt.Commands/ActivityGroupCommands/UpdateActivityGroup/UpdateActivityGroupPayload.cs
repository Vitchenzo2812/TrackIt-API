namespace TrackIt.Commands.ActivityGroupCommands.UpdateActivityGroup;

public record UpdateActivityGroupPayload (
  string Title,
  int Order
);