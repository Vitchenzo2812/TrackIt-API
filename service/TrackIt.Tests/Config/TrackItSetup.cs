using TrackIt.Infraestructure.Security.Contracts;
using Microsoft.Extensions.DependencyInjection;
using TrackIt.Infraestructure.Database;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using TrackIt.Entities.Core;
using MassTransit.Testing;
using TrackIt.Tests.Mocks;
using TrackIt.Entities;

namespace TrackIt.Tests.Config;

public abstract class TrackItSetup : IClassFixture<TrackItWebApplication>, IAsyncLifetime
{
  protected TrackItWebApplication _factory;

  protected HttpClient _httpClient;

  protected IJwtService _jwtService;

  protected IRefreshTokenService _refreshTokenService;

  protected TrackItDbContext _db;
  
  protected ITestHarness _harness;

  public TrackItSetup (TrackItWebApplication factory)
  {
    _factory = factory;
    _httpClient = _factory.CreateClient(
      new WebApplicationFactoryClientOptions
      {
        AllowAutoRedirect = false
      }
    );
    _jwtService = _factory.Services.GetService<IJwtService>()!;
    _refreshTokenService = _factory.Services.GetService<IRefreshTokenService>()!;
    _db = _factory.Services.GetRequiredService<TrackItDbContext>();
    _harness = _factory.Services.GetTestHarness();
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
  
  public async Task InitializeAsync ()
  {
    await _harness.Start();
    await _db.Database.EnsureDeletedAsync();
    await _db.Database.MigrateAsync(); 
  }

  public async Task DisposeAsync ()
  {
    _db.ChangeTracker.Clear();
    await _db.Database.EnsureDeletedAsync();
    await _harness.Stop();
  }
}