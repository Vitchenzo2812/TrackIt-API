using TrackIt.Entities.Core;

namespace TrackIt.Commands.ActivityCommands.DeleteActivity;

public class DeleteActivityCommand(ActivityAggregates activityAggregates, Session? session = null)
  : Command<ActivityAggregates, object>(activityAggregates, null, session);