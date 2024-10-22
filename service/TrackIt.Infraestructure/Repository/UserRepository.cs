using TrackIt.Infraestructure.Database;
using Microsoft.EntityFrameworkCore;
using TrackIt.Entities.Repository;
using TrackIt.Entities;

namespace TrackIt.Infraestructure.Repository;

public class UserRepository : IUserRepository
{
  private readonly TrackItDbContext _db;

  public UserRepository (TrackItDbContext db)
  {
    _db = db;
  }
  
  public async Task<User?> FindById (Guid aggregateId)
  {
    return await _db.User
      .AsTracking()
      .Include(u => u.Password)
      .FirstOrDefaultAsync(u => u.Id == aggregateId);
  }
  
  public User? FindByEmail (Email email)
  {
    return _db.User
      .Include(u => u.Password)
      .AsEnumerable()
      .FirstOrDefault(u => u.Email.Value == email.Value);
  }
  
  public void Save (User aggregate)
  {
    _db.User.Add(aggregate);
  }

  public void Delete (User aggregate)
  {
    _db.User.Remove(aggregate);
  }
}