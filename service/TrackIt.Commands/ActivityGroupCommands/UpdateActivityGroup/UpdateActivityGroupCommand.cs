using TrackIt.Entities.Core;

namespace TrackIt.Commands.ActivityGroupCommands.UpdateActivityGroup;

public class UpdateActivityGroupCommand(Guid aggregateId, UpdateActivityGroupPayload payload, Session? session = null)
  : Command<Guid, UpdateActivityGroupPayload>(aggregateId, payload, session);