using TrackIt.Infraestructure.Web.Dto;
using Microsoft.AspNetCore.Http;
using TrackIt.Entities.Errors;
using System.Text.Json;

namespace TrackIt.Infraestructure.Web.Middlewares;

public class GlobalExceptionMiddleware (RequestDelegate next)
{
  public async Task InvokeAsync (HttpContext context)
  {
    try
    {
      await next(context);
    }
    catch (Exception e)
    {
      await HandleExceptionAsync(context, e);
    }
  }

  private async Task HandleExceptionAsync (HttpContext context, Exception e)
  {
    context.Response.ContentType = "application/json";

    ApplicationError error = new InternalServerError(e.Message);

    if (e is ApplicationError)
      error = (ApplicationError)e;

    context.Response.StatusCode = error.HttpCode;

    var responseError = ErrorResponseDto.FromApplicationError(error);

    await context.Response.WriteAsync(JsonSerializer.Serialize(responseError));
  }
}