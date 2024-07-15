using TrackIt.Infraestructure.Security.Models;

namespace TrackIt.Infraestructure.Security.Contracts;

public interface ISessionService
{
  Task<Session> Create (Guid userId);

  Task<Session> Get (Guid userId, string oldToken);
}