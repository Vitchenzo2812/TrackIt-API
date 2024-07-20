using TrackIt.Infraestructure.Extensions;

namespace TrackIt.WebApi;

public class TrackItProgram
{
  public static void Main (string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);

    var startup = new WebApiTrackItStartup();
    
    startup.ConfigureServices(builder.Services);

    var app = builder.Build();
    
    startup.Configure(app, builder.Environment);

    app
      .MapDefaultEndpoints()
      .Run();
  }
}