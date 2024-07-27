using TrackIt.Entities.Core;

namespace TrackIt.Commands.Auth.ForgotPassword;

public class ForgotPasswordCommand (ForgotPasswordPayload payload)
  : Command<object, ForgotPasswordPayload, ForgotPasswordResponse>(null, payload);