using TrackIt.Entities.Core;

namespace TrackIt.Commands.ActivityCommands.CreateActivity;

public class CreateActivityCommand (Guid groupId, CreateActivityPayload payload, Session? session = null)
  : Command<Guid, CreateActivityPayload>(groupId, payload, session);