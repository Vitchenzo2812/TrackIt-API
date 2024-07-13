using TrackIt.Infraestructure.Database.Mappers;
using Microsoft.EntityFrameworkCore;
using TrackIt.Entities;

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
    
    optionsBuilder
      .UseMySql(
        "Server=localhost;Port=3306;Database=trackitservice;User=root;Password=password;SSL Mode=None;",
        new MySqlServerVersion(new Version())
      )
      .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
  }
  
  protected override void OnModelCreating (ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    
    new UserMapper().Configure(modelBuilder.Entity<User>());
  }
}