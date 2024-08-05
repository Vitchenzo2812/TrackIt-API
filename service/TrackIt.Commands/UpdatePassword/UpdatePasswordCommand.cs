using TrackIt.Entities.Core;

namespace TrackIt.Commands.UpdatePassword;

public class UpdatePasswordCommand (UpdatePasswordPayload payload, Session? session = null)
  : Command<UpdatePasswordPayload>(payload, session);