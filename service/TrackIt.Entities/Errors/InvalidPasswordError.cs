namespace TrackIt.Entities.Errors;

public class InvalidPasswordError : ApplicationError
{
  public InvalidPasswordError () : base(400, "INVALID_PASSWORD_FORMAT", "Invalid password format")
  {
  }
}