using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using TrackIt.Infraestructure.Web.Swagger.Annotations;

namespace TrackIt.Infraestructure.Web.Swagger;

public static class SwaggerAuthExtension
{
  private static readonly string authType = "Bearer JWT";

  private static readonly OpenApiSecurityRequirement requirement = new() 
  {{
    new OpenApiSecurityScheme
    {
      Reference = new OpenApiReference
      {
        Type = ReferenceType.SecurityScheme,
        Id = authType
      }
    },
    new string[]{}
  }};

  private static readonly OpenApiSecurityScheme scheme = new()
  {
    In = ParameterLocation.Header,
    Description = "Please enter a valid token",
    Name = "Authorization",
    Type = SecuritySchemeType.Http,
    BearerFormat = "JWT",
    Scheme = "Bearer"
  };

  public static SwaggerGenOptions AddJWTAuth (this SwaggerGenOptions option)
  {
    option.AddSecurityDefinition(authType, scheme);
    option.OperationFilter<SecurityRequirementsOperationFilter>();

    return option;
  }

  private class SecurityRequirementsOperationFilter : IOperationFilter
  {
    public void Apply (OpenApiOperation operation, OperationFilterContext context)
    {
      if (
        (context.MethodInfo.GetCustomAttributes(true).Any(x => x is SwaggerAuthorizeAttribute)) ||
        (context.MethodInfo.DeclaringType?.GetCustomAttributes(true).Any(x => x is SwaggerAuthorizeAttribute) ?? false)
      )
      {
        operation.Security = new List<OpenApiSecurityRequirement>
        {
          requirement
        };
      }
    }
  }
}