namespace TrackIt.Entities.Errors;

public class NotFoundError : ApplicationError
{
  public NotFoundError (string? message = "Not found") : base(404, "NOT_FOUND", message)
  {
  }
}