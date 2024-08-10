using TrackIt.Entities.Core;

namespace TrackIt.Commands.ActivityCommands.UpdateActivity;

public class UpdateActivityCommand (UpdateActivityAggregate aggregate, UpdateActivityPayload payload, Session? session = null)
  : Command<UpdateActivityAggregate, UpdateActivityPayload>(aggregate, payload, session);