using PartialSession = TrackIt.Entities.Core.Session;
using TrackIt.Infraestructure.Security.Contracts;
using TrackIt.Infraestructure.Security.Models;
using TrackIt.Infraestructure.Database;
using Microsoft.EntityFrameworkCore;
using TrackIt.Entities.Errors;
using TrackIt.Entities;

namespace TrackIt.Infraestructure.Security;

public class SessionService : ISessionService
{
  private readonly TrackItDbContext _db;

  private readonly IJwtService _jwtService;
  
  private readonly IRefreshTokenService _refreshTokenService;

  public SessionService (
    TrackItDbContext db,
    IJwtService jwtService,
    IRefreshTokenService refreshTokenService
  )
  {
    _db = db;
    _jwtService = jwtService;
    _refreshTokenService = refreshTokenService;
  }
  
  public async Task<Session> Create (Guid userId)
  {
    var user = await _db.User
      .Include(u => u.Password)
      .FirstOrDefaultAsync(u => u.Id == userId);

    if (user is null)
      throw new NotFoundError("User not found");

    var token = _jwtService.GenerateToken(PartialSession.Create(user));
    var refreshToken = await GetRefreshToken(user);
    
    return Session.Create(user, token, refreshToken);
  }

  public async Task<Session> Get (Guid userId, string oldToken)
  {
    var user = await _db.User
      .Include(u => u.Password)
      .FirstOrDefaultAsync(u => u.Id == userId);

    if (user is null)
      throw new NotFoundError("User not found");

    var oldRefreshToken = await _refreshTokenService.GetToken(user.Id);

    if (string.IsNullOrEmpty(oldRefreshToken))
      throw new NotFoundError("Failed to get old refresh token");

    return Session.Create(user, oldToken, oldRefreshToken);
  }

  private async Task<string> GetRefreshToken (User user)
  {
    var token = _refreshTokenService.GenerateToken();
    await _refreshTokenService.SaveToken(PartialSession.Create(user), token);

    return token;
  }
}