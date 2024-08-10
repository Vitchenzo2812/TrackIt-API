using TrackIt.Entities.Core;

namespace TrackIt.Commands.ActivityCommands.UpdateActivity;

public class UpdateActivityCommand (Guid aggregateId, UpdateActivityPayload payload, Session? session = null)
  : Command<Guid, UpdateActivityPayload>(aggregateId, payload, session);