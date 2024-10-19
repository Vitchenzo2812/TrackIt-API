using TrackIt.Entities;
using TrackIt.Entities.Activities;

namespace TrackIt.Commands.ActivityCommands.UpdateActivity;

public record UpdateActivityPayload (
  string Title,
  string? Description,
  ActivityPriority Priority,
  int Order,
  bool IsChecked
);