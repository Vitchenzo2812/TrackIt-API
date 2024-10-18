using TrackIt.Entities;

namespace TrackIt.Commands.ActivityCommands.UpdateActivity;

public record UpdateActivityPayload (
  string Title,
  string? Description,
  ActivityPriority Priority,
  int Order
);