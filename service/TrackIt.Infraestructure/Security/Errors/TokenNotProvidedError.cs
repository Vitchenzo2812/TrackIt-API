using TrackIt.Entities.Errors;

namespace TrackIt.Infraestructure.Security.Errors;

public class TokenNotProvidedError : ApplicationError
{
  public TokenNotProvidedError () : base(401, "TOKEN_NOT_PROVIDED", "Token not provided")
  {
  }
}