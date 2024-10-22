namespace TrackIt.Entities.Repository;

public interface IUserRepository : IRepository<User>
{
  User? FindByEmail (Email email);
}