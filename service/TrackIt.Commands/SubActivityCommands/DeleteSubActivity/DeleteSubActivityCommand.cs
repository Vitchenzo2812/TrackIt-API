using TrackIt.Entities.Core;

namespace TrackIt.Commands.SubActivityCommands.DeleteSubActivity;

public class DeleteSubActivityCommand(SubActivityAggregates aggregates, Session? session = null)
  : Command<SubActivityAggregates, object>(aggregates, null, session);