using TrackIt.Infraestructure.Database;
using Microsoft.EntityFrameworkCore;
using TrackIt.Entities.Activities;
using TrackIt.Entities.Repository;

namespace TrackIt.Infraestructure.Repository;

public class ActivityRepository : IActivityRepository
{
  private readonly TrackItDbContext _db;

  public ActivityRepository (TrackItDbContext db) => _db = db;
  
  public async Task<Activity?> FindById (Guid aggregateId)
  {
    return await _db.Activities
      .AsTracking()
      .Include(x => x.SubActivities)
      .FirstOrDefaultAsync(x => x.Id == aggregateId);
  }

  public void Save (Activity aggregate)
  {
    _db.Activities.Add(aggregate);
  }

  public void Delete (Activity aggregate)
  {
    _db.Activities.Remove(aggregate);
  }
}