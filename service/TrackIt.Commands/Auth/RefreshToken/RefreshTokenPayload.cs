namespace TrackIt.Commands.Auth.RefreshToken;

public record RefreshTokenPayload (
  string Token,
  
  string RefreshToken
);