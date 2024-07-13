using TrackIt.Infraestructure.Web.Swagger.Annotations;
using TrackIt.Infraestructure.Security.Contracts;
using TrackIt.Infraestructure.Security.Errors;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;

namespace TrackIt.Infraestructure.Web.Middlewares;

public class AuthorizationMiddleware
{
  private readonly RequestDelegate _next; 
    
  private readonly IJwtService _jwtService;

  public AuthorizationMiddleware (
    RequestDelegate next,
    IJwtService jwtService
  )
  {
    _next = next;
    _jwtService = jwtService;
  }
  
  public async Task InvokeAsync (HttpContext context)
  {
    var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
    
    if (endpoint?.Metadata.GetMetadata<SwaggerAuthorizeAttribute>() == null)
    {
      await _next(context);
      return;
    }
    
    var header = context.Request.Headers["Authorization"];
    var splited = header.ToString().Split(" ");
    
    if (splited.ElementAt(0) != "Bearer" || splited.ElementAt(1).IsNullOrEmpty())
      throw new InvalidTokenError();
    
    var token = splited.ElementAt(1);
    
    if (!_jwtService.Verify(token))
      throw new UnauthorizedError();
    
    var decoded = _jwtService.Decode(token);
    
    context.Request.Headers.Append(nameof(decoded.Id), decoded.Id.ToString());
    context.Request.Headers.Append(nameof(decoded.Email), decoded.Email);
    context.Request.Headers.Append(nameof(decoded.Name), decoded.Name);
    context.Request.Headers.Append(nameof(decoded.Income), decoded.Income.ToString());
    
    await _next(context);
  }
}