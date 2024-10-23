using TrackIt.Infraestructure.Database;
using Microsoft.EntityFrameworkCore;
using TrackIt.Entities.Repository;
using TrackIt.Entities.Expenses;

namespace TrackIt.Infraestructure.Repository;

public class CategoryConfigRepository : ICategoryConfigRepository
{
  private readonly TrackItDbContext _db;

  public CategoryConfigRepository (TrackItDbContext db) => _db = db; 
  
  public async Task<CategoryConfig?> FindById (Guid aggregateId)
  {
    return await _db.CategoryConfigs
      .AsTracking()
      .FirstOrDefaultAsync(x => x.Id == aggregateId);
  }

  public void Save (CategoryConfig aggregate)
  {
    _db.CategoryConfigs.Add(aggregate);
  }

  public void Delete (CategoryConfig aggregate)
  {
    _db.CategoryConfigs.Remove(aggregate);
  }
}