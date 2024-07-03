using TrackIt.Infraestructure.Web.Swagger.Annotations;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http;

namespace TrackIt.Infraestructure.Web.Middlewares;

public class AuthorizationMiddleware (
  RequestDelegate next
  // IJwt jwt
)
{
  public async Task InvokeAsync (HttpContext context)
  {
    // var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
    //
    // if (endpoint?.Metadata.GetMetadata<SwaggerAuthorizeAttribute>() == null)
    // {
    //   await next(context);
    //   return;
    // }
    //
    // var header = context.Request.Headers["Authorization"];
    // var splited = header.ToString().Split(" ");
    //
    // if (splited.ElementAt(0) != "Bearer" || splited.ElementAt(1).IsNullOrEmpty())
    //   throw new InvalidTokenError();
    //
    // var token = splited.ElementAt(1);
    //
    // if (!jwt.Verify(token))
    //   throw new UnauthorizedError();
    //
    // var decoded = jwt.Decode(token);
    //
    // context.Request.Headers.Append(nameof(decoded.Id), decoded.Id.ToString());
    // context.Request.Headers.Append(nameof(decoded.CompanyId), decoded.CompanyId?.ToString());
    // context.Request.Headers.Append(nameof(decoded.Name), decoded.Name);
    // context.Request.Headers.Append(nameof(decoded.Email), decoded.Email);
    // context.Request.Headers.Append(nameof(decoded.Picture), decoded.Picture);
    // context.Request.Headers.Append(nameof(decoded.Hierarchy), decoded.Hierarchy.Description());
    //
    // await next(context);
  }
}