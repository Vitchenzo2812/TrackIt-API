using TrackIt.Entities.Errors;

namespace TrackIt.Commands.Errors;

public class PasswordCannotBeTheSameError : ApplicationError
{
  public PasswordCannotBeTheSameError () : base(400, "PASSWORD_CANNOT_BE_THE_SAME", "Password cannot be the same!")
  {
  }
}