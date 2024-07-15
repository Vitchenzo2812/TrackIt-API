using TrackIt.Infraestructure.Security.Contracts;
using TrackIt.Infraestructure.Security.Models;
using Session = TrackIt.Entities.Core.Session;
using TrackIt.Infraestructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace TrackIt.Infraestructure.Security;

public class RefreshTokenService : IRefreshTokenService
{
  private readonly TrackItDbContext _db;

  public RefreshTokenService (TrackItDbContext db)
  {
    _db = db;
  }
  
  public string GenerateToken ()
  {
    var randomNumber = new byte[64];
    var rng = RandomNumberGenerator.Create();
    rng.GetBytes(randomNumber);
    return Convert.ToBase64String(randomNumber);
  }
  
  public async Task SaveToken (Session session, string refreshToken)
  {
    var oldTokens = await _db.RefreshToken.Where(r => r.UserId == session.Id).ToListAsync();

    if (oldTokens.Count != 0)
    {
      var oldToken = oldTokens.First();
      oldToken.Token = refreshToken;
      _db.RefreshToken.Entry(oldToken).State = EntityState.Modified;
      await _db.SaveChangesAsync();
      return;
    }

    _db.RefreshToken.Add(RefreshToken.Create(session.Id, refreshToken));
    await _db.SaveChangesAsync();
  }

  public async Task<string?> GetToken (Guid userId)
  {
    return (await _db.RefreshToken.FirstOrDefaultAsync(r => r.UserId == userId))?.Token;
  }
}