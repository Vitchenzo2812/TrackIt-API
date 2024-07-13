using TrackIt.Entities.Core;

namespace TrackIt.Commands.UpdateUser;

public class UpdateUserCommand (Guid aggregateId, UpdateUserPayload payload, Session? session = null)
  : Command<Guid, UpdateUserPayload>(aggregateId, payload, session);