using TrackIt.Entities.Core;
using Session = TrackIt.Infraestructure.Security.Models.Session;

namespace TrackIt.Commands.Auth.VerifyForgotPassword;

public class VerifyForgotPasswordCommand (VerifyForgotPasswordPayload payload)
  : Command<object, VerifyForgotPasswordPayload, Session>(null, payload);