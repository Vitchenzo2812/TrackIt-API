using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Infraestructure.Database;
using Microsoft.EntityFrameworkCore;
using TrackIt.Entities;

namespace TrackIt.Infraestructure.Repository;

public class ActivityRepository : IActivityRepository
{
  private readonly TrackItDbContext _db;

  public ActivityRepository (TrackItDbContext db)
  {
    _db = db;
  }
  
  public async Task<Activity?> FindById (Guid aggregateId)
  {
    return await _db.Activity
      .AsTracking()
      .Include(a => a.SubActivities)
      .FirstOrDefaultAsync(a => a.Id == aggregateId);
  }

  public async Task<List<Activity>> GetActivitiesByGroup (Guid activityGroupId)
  {
    return await _db.Activity
      .Include(a => a.SubActivities)
      .Where(a => a.ActivityGroupId == activityGroupId)
      .ToListAsync();
  }
  
  public void Save (Activity aggregate)
  {
    _db.Activity.Add(aggregate);
  }

  public void Delete (Activity aggregate)
  {
    _db.Activity.Remove(aggregate);
  }
}