namespace TrackIt.Commands.Auth.SignIn;

public record SignInPayload (
  string Email,
  
  string Password
);