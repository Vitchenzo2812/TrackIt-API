namespace TrackIt.Entities.Errors;

public class InternalServerError : ApplicationError
{
  public InternalServerError (string? message = "Internal server error") : base(500, "INTERNAL_SERVER_ERROR", message)
  {
  }
}