using TrackIt.Infraestructure.Database;
using Microsoft.EntityFrameworkCore;
using TrackIt.Entities.Repository;
using TrackIt.Entities.Expenses;

namespace TrackIt.Infraestructure.Repository;

public class CategoryRepository : ICategoryRepository
{
  private readonly TrackItDbContext _db;

  public CategoryRepository (TrackItDbContext db) => _db = db;
    
  public async Task<Category?> FindById (Guid aggregateId)
  {
    return await _db.Categories
      .AsTracking()
      .Include(x => x.CategoryConfig)
      .FirstOrDefaultAsync(x => x.Id == aggregateId);
  }

  public void Save (Category aggregate)
  {
    _db.Categories.Add(aggregate);
  }

  public void Delete (Category aggregate)
  {
    _db.Categories.Remove(aggregate);
  }
}