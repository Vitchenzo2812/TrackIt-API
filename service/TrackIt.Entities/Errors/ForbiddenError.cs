namespace TrackIt.Entities.Errors;

public class ForbiddenError : ApplicationError
{
  public ForbiddenError (string? message = "Forbidden Error") : base(403, "FORBIDDEN_ERROR", message)
  {
  }
}