using TrackIt.Entities.Core;

namespace TrackIt.Commands.Auth.SignUp;

public class SignUpCommand (SignUpPayload payload, Session? session = null)
  : Command<object, SignUpPayload, SignUpResponse>(null, payload, session);