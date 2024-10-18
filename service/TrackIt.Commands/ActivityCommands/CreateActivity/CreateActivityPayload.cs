using TrackIt.Entities;

namespace TrackIt.Commands.ActivityCommands.CreateActivity;

public record CreateActivityPayload (
  string Title,
  string? Description,
  ActivityPriority Priority,
  int Order
);