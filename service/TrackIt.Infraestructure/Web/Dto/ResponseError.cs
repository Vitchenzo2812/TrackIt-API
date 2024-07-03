using TrackIt.Entities.Errors;

namespace TrackIt.Infraestructure.Web.Dto;

public record ErrorResponseDto (
  int StatusCode,

  string Message,

  string Code
)
{
  public static ErrorResponseDto FromApplicationError (ApplicationError error)
  {
    return new ErrorResponseDto(
      StatusCode: error.HttpCode,
      
      Message: error.Message,
      
      Code: error.Code
    );
  }
}