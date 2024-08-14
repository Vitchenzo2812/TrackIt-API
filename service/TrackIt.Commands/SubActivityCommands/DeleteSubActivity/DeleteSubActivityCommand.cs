using TrackIt.Entities.Core;

namespace TrackIt.Commands.SubActivityCommands.DeleteSubActivity;

public class DeleteSubActivityCommand (DeleteSubActivityAggregate aggregate, Session? session = null)
  : Command<DeleteSubActivityAggregate, object>(aggregate, null, session);