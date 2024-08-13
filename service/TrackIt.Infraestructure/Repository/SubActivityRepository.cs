using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Infraestructure.Database;
using Microsoft.EntityFrameworkCore;
using TrackIt.Entities;

namespace TrackIt.Infraestructure.Repository;

public class SubActivityRepository : ISubActivityRepository
{
  private readonly TrackItDbContext _db;

  public SubActivityRepository (TrackItDbContext db)
  {
    _db = db;
  }
  
  public async Task<SubActivity?> FindById (Guid aggregateId)
  {
    return await _db.SubActivity
      .AsTracking()
      .FirstOrDefaultAsync(s => s.Id == aggregateId) ;
  }

  public void Save (SubActivity aggregate)
  {
    _db.SubActivity.Add(aggregate);
  }

  public void Delete (SubActivity aggregate)
  {
    _db.SubActivity.Remove(aggregate);
  }
}