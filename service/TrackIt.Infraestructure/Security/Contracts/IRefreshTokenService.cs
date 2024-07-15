using TrackIt.Entities.Core;

namespace TrackIt.Infraestructure.Security.Contracts;

public interface IRefreshTokenService
{
  string GenerateToken ();

  Task SaveToken (Session session, string refreshToken);

  Task<string?> GetToken (Guid userId);
}