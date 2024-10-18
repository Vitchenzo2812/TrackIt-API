namespace TrackIt.Commands.SubActivityCommands;

public record SubActivityAggregates (
  Guid GroupId,
  Guid ActivityId,
  Guid? SubActivityId = null
);