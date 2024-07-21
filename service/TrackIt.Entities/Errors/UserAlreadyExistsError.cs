namespace TrackIt.Entities.Errors;

public class UserAlreadyExistsError : ApplicationError
{
  public UserAlreadyExistsError () : base(400, "USER_ALREADY_EXISTS", "User already exists!")
  {
  }
}