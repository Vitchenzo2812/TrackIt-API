using TrackIt.Entities.Errors;

namespace TrackIt.Infraestructure.Security.Errors;

public class InvalidTokenError : ApplicationError
{
  public InvalidTokenError () : base(401, "INVALID_TOKEN", "Invalid token")
  {
  }
}