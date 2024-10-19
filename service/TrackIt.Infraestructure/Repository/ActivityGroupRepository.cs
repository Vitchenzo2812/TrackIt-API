using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Infraestructure.Database;
using Microsoft.EntityFrameworkCore;
using TrackIt.Entities;
using TrackIt.Entities.Activities;

namespace TrackIt.Infraestructure.Repository;

public class ActivityGroupRepository : IActivityGroupRepository
{
  private readonly TrackItDbContext _db;

  public ActivityGroupRepository (TrackItDbContext db) => _db = db;

  public async Task<List<ActivityGroup>> GetAll ()
  {
    return await _db.ActivityGroups.ToListAsync();
  }
  
  public async Task<ActivityGroup?> FindById (Guid aggregateId)
  {
    return await _db.ActivityGroups
      .AsTracking()
      .Include(x => x.Activities)
      .FirstOrDefaultAsync(x => x.Id == aggregateId);
  }

  public void Save (ActivityGroup aggregate)
  {
    _db.ActivityGroups.Add(aggregate);
  }

  public void Delete (ActivityGroup aggregate)
  {
    _db.ActivityGroups.Remove(aggregate);
  }
}