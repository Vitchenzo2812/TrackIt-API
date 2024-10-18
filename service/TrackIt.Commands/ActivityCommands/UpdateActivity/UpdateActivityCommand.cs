using TrackIt.Entities.Core;

namespace TrackIt.Commands.ActivityCommands.UpdateActivity;

public class UpdateActivityCommand(Aggregates aggregates, UpdateActivityPayload payload, Session? session = null)
  : Command<Aggregates, UpdateActivityPayload>(aggregates, payload, session);