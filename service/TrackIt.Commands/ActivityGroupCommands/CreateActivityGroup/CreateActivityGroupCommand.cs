using TrackIt.Entities.Core;

namespace TrackIt.Commands.ActivityGroupCommands.CreateActivityGroup;

public class CreateActivityGroupCommand (CreateActivityGroupPayload payload, Session? session = null)
  : Command<CreateActivityGroupPayload>(payload, session);