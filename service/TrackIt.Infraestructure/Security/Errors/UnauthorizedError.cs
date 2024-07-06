using TrackIt.Entities.Errors;

namespace TrackIt.Infraestructure.Security.Errors;

public class UnauthorizedError : ApplicationError
{
  public UnauthorizedError () : base(401, "UNAUTHORIZED", "Unauthorized")
  {
  }
}