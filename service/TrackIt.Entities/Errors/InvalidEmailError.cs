namespace TrackIt.Entities.Errors;

public class InvalidEmailError : ApplicationError
{
  public InvalidEmailError () : base(400, "INVALID_EMAIL_FORMAT", "Invalid email format")
  {
  }
}