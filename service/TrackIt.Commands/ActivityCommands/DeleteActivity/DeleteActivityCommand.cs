using TrackIt.Entities.Core;

namespace TrackIt.Commands.ActivityCommands.DeleteActivity;

public class DeleteActivityCommand (DeleteActivityAggreagate aggregate, Session? session = null)
  : Command<DeleteActivityAggreagate, object>(aggregate, null, session);