using TrackIt.Entities.Errors;

namespace TrackIt.Commands.Errors;

public class EmailAlreadyInUseError : ApplicationError
{
  public EmailAlreadyInUseError () : base(400, "EMAIL_ALREADY_IN_USE", "Email already in use!")
  {
  }
}