using TrackIt.Infraestructure.Database.Mappers;
using Microsoft.EntityFrameworkCore;
using TrackIt.Entities;
using TrackIt.Entities.Errors;
using TrackIt.Infraestructure.Config;
using TrackIt.Infraestructure.Extensions;

namespace TrackIt.Infraestructure.Database;

public class TrackItDbContext : DbContext
{
  public static bool IsMigration { get; set;  } = true;
  
  public DbSet<User> User { get; set; }

  public DbSet<Password> Password { get; set; }
  
  public TrackItDbContext (DbContextOptions<TrackItDbContext> options) : base(options)
  {
  }

  protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder)
  {
    if (!IsMigration) return;

    string connection = Environment.GetEnvironmentVariable(EnvironmentVariables.MySqlTrackItConnectionString.Description());

    if (string.IsNullOrEmpty(connection))
      throw new InternalServerError("Connection string not found");
    
    optionsBuilder
      .UseMySql(
        connection,
        new MySqlServerVersion(new Version())
      )
      .EnableSensitiveDataLogging()
      .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
  }
  
  protected override void OnModelCreating (ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    
    new UserMapper().Configure(modelBuilder.Entity<User>());
  }
}