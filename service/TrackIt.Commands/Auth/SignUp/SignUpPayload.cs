namespace TrackIt.Commands.Auth.SignUp;

public record SignUpPayload (
  string Email,
  
  string Password
);