using TrackIt.Entities.Core;

namespace TrackIt.Infraestructure.Security.Contracts;

public interface IJwtService
{
  string GenerateToken (Session session);

  bool Verify (string token);

  Session Decode (string token);
}