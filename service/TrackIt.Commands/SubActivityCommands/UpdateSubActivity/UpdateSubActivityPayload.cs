using TrackIt.Entities;
using TrackIt.Entities.Activities;

namespace TrackIt.Commands.SubActivityCommands.UpdateSubActivity;

public record UpdateSubActivityPayload (
  string Title,
  string? Description,
  ActivityPriority Priority,
  int Order,
  bool IsChecked
);