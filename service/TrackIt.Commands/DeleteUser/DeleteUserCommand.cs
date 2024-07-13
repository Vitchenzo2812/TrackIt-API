using TrackIt.Entities.Core;

namespace TrackIt.Commands.DeleteUser;

public class DeleteUserCommand (Guid aggregateId, Session? session = null)
  : Command<Guid, object>(aggregateId, null, session);