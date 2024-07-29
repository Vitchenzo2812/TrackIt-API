using TrackIt.Entities.Core;

namespace TrackIt.Commands.Auth.VerifyForgotPassword;

public class VerifyForgotPasswordCommand (VerifyForgotPasswordPayload payload)
  : Command<VerifyForgotPasswordPayload>(payload);