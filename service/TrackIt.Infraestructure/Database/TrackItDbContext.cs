using Microsoft.EntityFrameworkCore;

namespace TrackIt.Infraestructure.Database;

public class TrackItDbContext (DbContextOptions options) : DbContext (options)
{
}