using TrackIt.Entities.Core;

namespace TrackIt.Commands.SubActivityCommands.UpdateSubActivity;

public class UpdateSubActivityCommand(SubActivityAggregates aggregates, UpdateSubActivityPayload payload, Session? session = null)
  : Command<SubActivityAggregates, UpdateSubActivityPayload>(aggregates, payload, session);