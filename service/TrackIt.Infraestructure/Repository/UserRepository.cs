using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Infraestructure.Database;
using Microsoft.EntityFrameworkCore;
using TrackIt.Entities;

namespace TrackIt.Infraestructure.Repository;

public class UserRepository (TrackItDbContext db) : IUserRepository
{
  public async Task<User?> FindById (Guid aggregateId)
  {
    return (await db.User.FirstOrDefaultAsync(u => u.Id == aggregateId));
  }

  public async Task<User?> FindByEmail (Email email)
  {
    return (await db.User.FirstOrDefaultAsync(u => u.Email.Value == email.Value));
  }

  public void Save (User aggregate)
  {
    db.User.Add(aggregate);
  }

  public void Delete (User aggregate)
  {
    db.User.Remove(aggregate);
  }
  
  public void Update (User aggregate)
  {
  }
}