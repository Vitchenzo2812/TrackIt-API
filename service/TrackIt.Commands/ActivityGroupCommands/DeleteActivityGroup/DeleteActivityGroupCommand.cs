using TrackIt.Entities.Core;

namespace TrackIt.Commands.ActivityGroupCommands.DeleteActivityGroup;

public class DeleteActivityGroupCommand(Guid aggregateId, Session? session = null)
  : Command<Guid, object>(aggregateId, null, session);