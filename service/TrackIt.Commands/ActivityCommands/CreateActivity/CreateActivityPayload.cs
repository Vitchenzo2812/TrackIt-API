using TrackIt.Entities;
using TrackIt.Entities.Activities;

namespace TrackIt.Commands.ActivityCommands.CreateActivity;

public record CreateActivityPayload (
  string Title,
  string? Description,
  ActivityPriority Priority,
  int Order
);