namespace TrackIt.Commands.Auth.VerifyForgotPassword;

public record VerifyForgotPasswordPayload (
  Guid UserId,
  
  string Code
);