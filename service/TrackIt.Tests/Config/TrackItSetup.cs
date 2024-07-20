using TrackIt.Infraestructure.Security.Contracts;
using Microsoft.Extensions.DependencyInjection;
using TrackIt.Infraestructure.Database;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using TrackIt.Tests.Mocks.Entities;
using TrackIt.Entities.Core;
using MassTransit.Testing;
using TrackIt.Entities;

namespace TrackIt.Tests.Config;

public abstract class TrackItSetup : IClassFixture<TrackItWebApplication>, IDisposable
{
  protected TrackItWebApplication _factory;
  
  private IServiceScope _scope;

  protected HttpClient _httpClient;

  protected IJwtService _jwtService;

  protected IRefreshTokenService _refreshTokenService;

  protected TrackItDbContext _db;
  
  protected ITestHarness _harness;
  
  protected TrackItSetup (TrackItWebApplication factory)
  {
    _factory = factory;
    _scope = factory.Services.CreateScope();
    _httpClient = _factory.CreateClient(
      new WebApplicationFactoryClientOptions
      {
        AllowAutoRedirect = false
      }
    );
    _jwtService = _scope.ServiceProvider.GetService<IJwtService>()!;
    _refreshTokenService = _scope.ServiceProvider.GetService<IRefreshTokenService>()!;
    _db = _scope.ServiceProvider.GetRequiredService<TrackItDbContext>();
    _harness = _scope.ServiceProvider.GetTestHarness();

    _harness.Start().Wait();

    _db.Database.EnsureDeleted();
    _db.Database.Migrate();
  }
  
  protected void AddAuthorizationData (Session s)
  {
    _httpClient.DefaultRequestHeaders.Remove("Authorization");
    var token = _jwtService.GenerateToken(s);
    _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
  }

  protected async Task<UserMock> CreateUser ()
  {
    var user = UserMock.Build(Password.Create("PasswordTest@1234"));
    _db.User.Add(user);

    await _db.SaveChangesAsync();

    return user;
  }
  
  protected async Task<UserMock> CreateUserWithEmailValidated ()
  {
    var user = UserMock.Build(Password.Create("PasswordTest@1234")).WithEmailValidated();
    _db.User.Add(user);

    await _db.SaveChangesAsync();

    return user;
  }
  
  protected async Task<UserMock> CreateAdminUser ()
  {
    var user = UserMock.Build(Password.Create("AdminPassword@1234")).MakeAdministrator();
    _db.User.Add(user);

    await _db.SaveChangesAsync();

    return user;
  }
  
  public void Dispose()
  {
    _harness.Stop();
    _scope.Dispose();
    _db.Dispose();
  }
}