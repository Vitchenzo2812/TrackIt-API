using TrackIt.Entities.Core;

namespace TrackIt.Commands.SubActivityCommands.UpdateSubActivity;

public class UpdateSubActivityCommand (UpdateSubActivityAggregate aggregate, UpdateSubActivityPayload payload, Session? session = null)
  : Command<UpdateSubActivityAggregate, UpdateSubActivityPayload>(aggregate, payload, session);