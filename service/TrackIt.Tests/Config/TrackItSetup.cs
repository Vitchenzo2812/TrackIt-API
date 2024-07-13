using TrackIt.Infraestructure.Security.Contracts;
using Microsoft.Extensions.DependencyInjection;
using TrackIt.Infraestructure.Database;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using TrackIt.Entities;
using TrackIt.Entities.Core;
using TrackIt.Tests.Mocks;

namespace TrackIt.Tests.Config;

public class TrackItSetup : IClassFixture<TrackItWebApplication>, IAsyncLifetime
{
  protected TrackItWebApplication _factory;

  protected HttpClient _httpClient;

  protected IJwtService _jwtService;

  protected TrackItDbContext _db;

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
    _db = _factory.Services.GetRequiredService<TrackItDbContext>();
  }
  
  protected void AddAuthorizationData (Session s)
  {
    _httpClient.DefaultRequestHeaders.Remove("Authorization");
    var token = _jwtService.GenerateToken(s);
    _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
  }

  protected async Task<UserMock> CreateUser ()
  {
    var password = Password.Create("PasswordTest@1234");
    _db.Password.Add(password);
    
    var user = UserMock.Build(password);
    _db.User.Add(user);

    await _db.SaveChangesAsync();

    return user;
  }
  
  protected async Task<UserMock> CreateAdminUser ()
  {
    var password = Password.Create("AdminPassword@1234");
    _db.Password.Add(password);
    
    var user = UserMock.Build(password).MakeAdministrator();
    _db.User.Add(user);

    await _db.SaveChangesAsync();

    return user;
  }
  
  public async Task InitializeAsync ()
  {
    await _db.Database.MigrateAsync();
  }

  public async Task DisposeAsync ()
  {
    _db.ChangeTracker.Clear();
    await _db.Database.EnsureDeletedAsync();
  }
}