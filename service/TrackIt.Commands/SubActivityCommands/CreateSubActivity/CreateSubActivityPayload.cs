using TrackIt.Entities;
using TrackIt.Entities.Activities;

namespace TrackIt.Commands.SubActivityCommands.CreateSubActivity;

public record CreateSubActivityPayload (
  string Title,
  string? Description,
  ActivityPriority Priority,
  int Order
);