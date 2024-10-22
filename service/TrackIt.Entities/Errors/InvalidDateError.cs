namespace TrackIt.Entities.Errors;

public class InvalidDateError : ApplicationError
{
  public InvalidDateError () : base(400, "INVALID_DATE", "Invalid date")
  {
  }
}