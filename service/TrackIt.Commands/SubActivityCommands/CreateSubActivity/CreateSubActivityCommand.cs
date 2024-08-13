using TrackIt.Entities.Core;

namespace TrackIt.Commands.SubActivityCommands.CreateSubActivity;

public class CreateSubActivityCommand (CreateSubActivityAggregate aggregate, CreateSubActivityPayload payload, Session? session = null)
  : Command<CreateSubActivityAggregate, CreateSubActivityPayload>(aggregate, payload, session);