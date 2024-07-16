using Session = TrackIt.Infraestructure.Security.Models.Session;
using PartialSession = TrackIt.Entities.Core.Session;
using TrackIt.Entities.Core;

namespace TrackIt.Commands.Auth.RefreshToken;

public class RefreshTokenCommand (RefreshTokenPayload payload, PartialSession? session = null)
  : Command<object, RefreshTokenPayload, Session>(null, payload, session);