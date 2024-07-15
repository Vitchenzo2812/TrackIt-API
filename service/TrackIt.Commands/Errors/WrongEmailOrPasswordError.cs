using TrackIt.Entities.Errors;

namespace TrackIt.Commands.Errors;

public class WrongEmailOrPasswordError : ApplicationError
{
  public WrongEmailOrPasswordError () : base(400, "WRONG_EMAIL_OR_PASSWORD", "Wrong email or password!")
  {
  }
}