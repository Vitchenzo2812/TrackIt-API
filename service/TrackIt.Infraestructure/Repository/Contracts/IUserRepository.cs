using TrackIt.Entities;

namespace TrackIt.Infraestructure.Repository.Contracts;

public interface IUserRepository : IRepository<User>
{
  Task<User?> FindByEmail (Email email);
}