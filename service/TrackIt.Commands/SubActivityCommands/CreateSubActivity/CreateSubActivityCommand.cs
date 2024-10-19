using TrackIt.Entities.Core;

namespace TrackIt.Commands.SubActivityCommands.CreateSubActivity;

public class CreateSubActivityCommand(SubActivityAggregates subActivityAggregates, CreateSubActivityPayload payload, Session? session = null)
  : Command<SubActivityAggregates, CreateSubActivityPayload>(subActivityAggregates, payload, session);