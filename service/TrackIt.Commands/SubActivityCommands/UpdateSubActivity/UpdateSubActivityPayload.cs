using TrackIt.Entities;

namespace TrackIt.Commands.SubActivityCommands.UpdateSubActivity;

public record UpdateSubActivityPayload (
  string Title,
  string? Description,
  ActivityPriority Priority,
  int Order,
  bool IsChecked
);