using TrackIt.Infraestructure.Security.Contracts;
using Microsoft.Extensions.DependencyInjection;
using TrackIt.Infraestructure.Database;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using TrackIt.Entities.Core;

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