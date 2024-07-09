
using Microsoft.AspNetCore.Mvc.Testing;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Hosting;
using Testcontainers.MySql;
using TrackIt.WebApi;

namespace TrackIt.Tests.Config;

public class TrackItWebApplication : WebApplicationFactory<TrackItProgram>, IAsyncLifetime
{
  protected MySqlContainer _baseDb = new MySqlBuilder().WithDatabase("trackitservice").Build();
  
  protected override void ConfigureWebHost (IWebHostBuilder builder)
  {
    var connectionString = _baseDb.GetConnectionString();
    
    Environment.SetEnvironmentVariable(
      "MYSQL_TRACKIT_CONNECTION_STRING",
      connectionString
    );

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

  public async Task DisposeAsync ()
  {
    await _baseDb.StopAsync();
  }
}