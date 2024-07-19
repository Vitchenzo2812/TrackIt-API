using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Hosting;
using TrackIt.Events.Consumers;
using Testcontainers.MySql;
using TrackIt.WebApi;
using MassTransit;
using MySqlConnector;

namespace TrackIt.Tests.Config;

public class TrackItWebApplication : WebApplicationFactory<TrackItProgram>, IAsyncLifetime
{
  private MySqlContainer _baseDb { get; }

  public TrackItWebApplication ()
  {
    _baseDb = new MySqlBuilder().WithDatabase("trackitservice").Build();
  }
  
  protected override void ConfigureWebHost (IWebHostBuilder builder)
  {
    var connectionString = _baseDb.GetConnectionString();
    
    Environment.SetEnvironmentVariable(
      "MYSQL_TRACKIT_CONNECTION_STRING",
      connectionString
    );

    builder.ConfigureTestServices(services =>
    {
      services.AddMassTransitTestHarness(x =>
      {
        x.AddConsumers(typeof(SignUpEventConsumer).Assembly);

        x.SetTestTimeouts(testInactivityTimeout: TimeSpan.FromSeconds(60));

        x.UsingInMemory((ctx, cfg) =>
        {
          cfg.ConfigureEndpoints(ctx);
          cfg.UseConcurrencyLimit(1);
        });
      });
    });
    
    builder.UseEnvironment("Tests");
    
    var randomNumber = new byte[64];
    var rng = RandomNumberGenerator.Create();
    rng.GetBytes(randomNumber);
    var secret = Convert.ToBase64String(randomNumber);
    Environment.SetEnvironmentVariable("ENVIRONMENT", "Tests");
    Environment.SetEnvironmentVariable("JWT_SECRET", secret);
  }
  
  public async Task InitializeAsync ()
  {
    await _baseDb.StartAsync();
  }

  public new async Task DisposeAsync ()
  {
    await base.DisposeAsync();
    
    await _baseDb.StopAsync();
  }
}