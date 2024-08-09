using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Infraestructure.Database;
using Microsoft.EntityFrameworkCore;
using TrackIt.Entities;

namespace TrackIt.Infraestructure.Repository;

public class ActivityGroupRepository : IActivityGroupRepository
{
  private readonly TrackItDbContext _db;

  public ActivityGroupRepository (
    TrackItDbContext db
  )
  {
    _db = db;
  }

  public async Task<List<ActivityGroup>> GetAll ()
  {
    return await _db.ActivityGroup.ToListAsync();
  }
  
  public async Task<ActivityGroup?> FindById (Guid aggregateId)
  {
    return await _db.ActivityGroup
      .AsTracking()
      .Include(aG => aG.Activities)
      .FirstOrDefaultAsync(aG => aG.Id == aggregateId);
  }

  public void Save (ActivityGroup aggregate)
  {
    _db.ActivityGroup.Add(aggregate);
  }

  public void Delete (ActivityGroup aggregate)
  {
    _db.ActivityGroup.Remove(aggregate);
  }
}