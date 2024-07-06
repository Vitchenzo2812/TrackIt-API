using TrackIt.Infraestructure.Database.Mappers;
using Microsoft.EntityFrameworkCore;
using TrackIt.Entities;

namespace TrackIt.Infraestructure.Database;

public class TrackItDbContext : DbContext
{
  public DbSet<User> User;

  public DbSet<Password> Password;
  
  public TrackItDbContext (DbContextOptions<TrackItDbContext> options) : base(options)
  {
  }

  protected override void OnModelCreating (ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    
    new UserMapper().Configure(modelBuilder.Entity<User>());
  }
}