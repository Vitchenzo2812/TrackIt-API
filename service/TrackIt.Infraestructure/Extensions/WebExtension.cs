using Microsoft.AspNetCore.Builder;

namespace TrackIt.Infraestructure.Extensions;

public static class WebExtension
{
  public static WebApplication MapDefaultEndpoints (this WebApplication app)
  {
    return app;
  }
}