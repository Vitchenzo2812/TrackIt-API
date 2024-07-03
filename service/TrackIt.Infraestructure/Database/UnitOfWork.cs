using TrackIt.Infraestructure.Database.Contracts;

namespace TrackIt.Infraestructure.Database;

public class UnitOfWork (TrackItDbContext db) : IUnitOfWork
{
  public async Task SaveChangesAsync ()
  {
    await db.SaveChangesAsync();
  }
}