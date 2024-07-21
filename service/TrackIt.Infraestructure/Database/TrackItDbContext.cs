using TrackIt.Infraestructure.Database.Mappers;
using Microsoft.EntityFrameworkCore;
using TrackIt.Entities;
using TrackIt.Entities.Core;
using TrackIt.Infraestructure.Security.Models;

namespace TrackIt.Infraestructure.Database;

public class TrackItDbContext : DbContext
{
  public static bool IsMigration { get; set; } = true;

  public DbSet<User> User { get; init; }

  public DbSet<Password> Password { get; init; }
  
  public DbSet<Ticket> Ticket { get; init; }
  
  public DbSet<TicketCode> TicketCode { get; init; }
  
  public DbSet<RefreshToken> RefreshToken { get; init; }
  
  public TrackItDbContext (DbContextOptions<TrackItDbContext> options) : base(options)
  {
  }

  protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder)
  {
    if (!IsMigration) return;
    
    optionsBuilder
      .UseMySql(
        Environment.GetEnvironmentVariable("MYSQL_TRACKIT_CONNECTION_STRING"),
        new MySqlServerVersion(new Version()),
        opt => opt.EnableRetryOnFailure()
      )
      .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
  }
  
  protected override void OnModelCreating (ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    modelBuilder
      .Entity<User>()
      .HasOne(u => u.Password)
      .WithOne(p => p.User)
      .HasForeignKey<Password>(p => p.UserId)
      .OnDelete(DeleteBehavior.Cascade);
    
    new UserMapper().Configure(modelBuilder.Entity<User>());
  }
}