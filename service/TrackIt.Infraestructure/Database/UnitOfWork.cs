using TrackIt.Infraestructure.Database.Contracts;

namespace TrackIt.Infraestructure.Database;

public class UnitOfWork : IUnitOfWork
{
  private readonly TrackItDbContext _db;

  public UnitOfWork (TrackItDbContext db) => _db = db;
  
  public async Task SaveChangesAsync ()
  {
    await _db.SaveChangesAsync();
  }
}