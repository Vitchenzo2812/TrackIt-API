namespace TrackIt.Entities.Errors;

public class InvalidPasswordError : ApplicationError
{
  public InvalidPasswordError (string? message = "Invalid password format") : base(400, "INVALID_PASSWORD_FORMAT", message)
  {
  }
}