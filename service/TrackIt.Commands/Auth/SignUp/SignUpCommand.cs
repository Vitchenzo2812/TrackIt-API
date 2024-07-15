using Session = TrackIt.Infraestructure.Security.Models.Session;
using PartialSession = TrackIt.Entities.Core.Session;
using TrackIt.Entities.Core;

namespace TrackIt.Commands.Auth.SignUp;

public class SignUpCommand (SignUpPayload payload, PartialSession? session = null)
  : Command<object, SignUpPayload, Session>(null, payload, session);