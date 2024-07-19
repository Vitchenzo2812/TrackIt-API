using TrackIt.Entities.Errors;

namespace TrackIt.Commands.Errors;

public class EmailMustBeValidatedError : ApplicationError
{
  public EmailMustBeValidatedError () : base(400, "EMAIL_MUST_BE_VALIDATED", "Email must be validated!")
  {
  }
}