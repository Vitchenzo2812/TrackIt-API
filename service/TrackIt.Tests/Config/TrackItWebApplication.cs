using Microsoft.Extensions.DependencyInjection;
using TrackIt.Infraestructure.Mailer.Contracts;
using TrackIt.Infraestructure.Mailer.Models;
using TrackIt.Infraestructure.Extensions;
using Microsoft.AspNetCore.Mvc.Testing;
using TrackIt.Infraestructure.Database;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Hosting;
using TrackIt.Tests.Mocks.Infra;
using TrackIt.Events.Consumers;
using Testcontainers.MySql;
using TrackIt.WebApi;
using MassTransit;
using Moq;
using TrackIt.Infraestructure.Database.Interceptor;

namespace TrackIt.Tests.Config;

public class TrackItWebApplication : WebApplicationFactory<TrackItProgram>, IAsyncLifetime
{
  private readonly MySqlContainer _baseDb = new MySqlBuilder()
    .WithImage("mysql:9.0")
    .WithDatabase("trackitservice")
    .WithUsername("root")
    .WithPassword("password")
    .WithCleanUp(true)
    .Build();

  public Mock<IMailerService> MailerServiceMock = new Mock<IMailerService>();
  
  protected override void ConfigureWebHost (IWebHostBuilder builder)
  {
    builder.ConfigureTestServices(services =>
    {
      services.RemoveDbContext<TrackItDbContext>();
      services.AddDbContext<TrackItDbContext>((sp, opt) =>
      {
        opt
          .AddInterceptors(sp.GetRequiredService<PublishEvents>())
          .UseMySql(
          _baseDb.GetConnectionString(), 
          new MySqlServerVersion(new Version()),
          option => option.EnableRetryOnFailure()
        );
      });
      
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

      services.AddTransient<IMailerService, MailerServiceMock>();
      MailerServiceMock.Setup(m => m.Send(It.IsAny<MailRequest>())).Returns(Task.CompletedTask);
      services.AddSingleton(MailerServiceMock.Object);
    });
    
    builder.UseEnvironment("Tests");
    
    var randomNumber = new byte[64];
    var rng = RandomNumberGenerator.Create();
    rng.GetBytes(randomNumber);
    var secret = Convert.ToBase64String(randomNumber);
    Environment.SetEnvironmentVariable("ENVIRONMENT", "Tests");
    Environment.SetEnvironmentVariable("JWT_SECRET", secret);
  }

  public Task InitializeAsync ()
  {
    return _baseDb.StartAsync();
  }

  public new Task DisposeAsync ()
  {
    return _baseDb.StopAsync();
  }
}