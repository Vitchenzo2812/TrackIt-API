using Session = TrackIt.Infraestructure.Security.Models.Session;
using PartialSession = TrackIt.Entities.Core.Session;
using TrackIt.Entities.Core;

namespace TrackIt.Commands.Auth.SignIn;

public class SignInCommand (SignInPayload payload, PartialSession? session = null)
  : Command<object, SignInPayload, Session>(null, payload, session);