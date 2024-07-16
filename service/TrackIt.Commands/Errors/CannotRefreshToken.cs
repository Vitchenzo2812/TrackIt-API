using TrackIt.Entities.Errors;

namespace TrackIt.Commands.Errors;

public class CannotRefreshToken : ApplicationError
{
  public CannotRefreshToken () : base(401, "CANNOT_REFRESH_TOKEN", "Invalid refresh token request")
  {
  }
}