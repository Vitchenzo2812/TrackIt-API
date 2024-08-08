using TrackIt.Entities.Core;

namespace TrackIt.Commands.ActivityCommands.CreateActivity;

public class CreateActivityCommand (Guid aggregateId, CreateActivityPayload payload, Session? session = null)
  : Command<Guid, CreateActivityPayload>(aggregateId, payload, session);