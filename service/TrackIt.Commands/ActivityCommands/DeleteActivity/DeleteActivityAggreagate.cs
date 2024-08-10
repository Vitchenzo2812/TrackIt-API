namespace TrackIt.Commands.ActivityCommands.DeleteActivity;

public record DeleteActivityAggreagate (
  Guid Id,
  
  Guid EntityId
);