using TrackIt.Entities.Core;

namespace TrackIt.Commands.ActivityCommands.UpdateActivity;

public class UpdateActivityCommand(ActivityAggregates activityAggregates, UpdateActivityPayload payload, Session? session = null)
  : Command<ActivityAggregates, UpdateActivityPayload>(activityAggregates, payload, session);