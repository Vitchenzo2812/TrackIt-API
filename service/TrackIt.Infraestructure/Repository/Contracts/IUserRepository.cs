using TrackIt.Entities;

namespace TrackIt.Infraestructure.Repository.Contracts;

public interface IUserRepository : IRepository<User>
{
  User? FindByEmail (Email email);
}