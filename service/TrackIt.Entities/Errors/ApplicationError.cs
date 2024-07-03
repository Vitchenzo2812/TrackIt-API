namespace TrackIt.Entities.Errors;

public class ApplicationError : Exception
{
  public int HttpCode { get; set; }
  
  public string Code { get; set; }

  public ApplicationError (int httpCode, string code, string? message) : base(message)
  {
    HttpCode = httpCode;
    Code = code;
  }
}