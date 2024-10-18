using TrackIt.Entities.Core;

namespace TrackIt.Commands.ActivityCommands.DeleteActivity;

public class DeleteActivityCommand(Aggregates aggregates, Session? session = null)
  : Command<Aggregates, object>(aggregates, null, session);