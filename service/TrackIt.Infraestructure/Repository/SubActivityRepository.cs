﻿using TrackIt.Infraestructure.Database;
using Microsoft.EntityFrameworkCore;
using TrackIt.Entities.Activities;
using TrackIt.Entities.Repository;

namespace TrackIt.Infraestructure.Repository;

public class SubActivityRepository : ISubActivityRepository
{
  private readonly TrackItDbContext _db;

  public SubActivityRepository (TrackItDbContext db) => _db = db; 
  
  public async Task<SubActivity?> FindById (Guid aggregateId)
  {
    return await _db.SubActivities
      .AsTracking()
      .FirstOrDefaultAsync(x => x.Id == aggregateId);
  }

  public async Task<List<SubActivity>> FindByActivityId (Guid activityId)
  {
    return await _db.SubActivities
      .Where(x => x.ActivityId == activityId)
      .ToListAsync();
  }

  public void Save (SubActivity aggregate)
  {
    _db.SubActivities.Add(aggregate);
  }

  public void Delete (SubActivity aggregate)
  {
    _db.SubActivities.Remove(aggregate);
  }
}