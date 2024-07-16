using TrackIt.Entities.Core;

namespace TrackIt.Commands.Auth.SignUp;

public class SignUpCommand (SignUpPayload payload, Session? session = null)
  : Command<SignUpPayload>(payload, session);