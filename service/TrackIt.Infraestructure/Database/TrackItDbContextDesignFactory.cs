using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TrackIt.Infraestructure.Database;

public class TrackItDbContextDesignFactory : IDesignTimeDbContextFactory<TrackItDbContext>
{
  public TrackItDbContext CreateDbContext (string[] args)
  {
    var optionsBuilder = new DbContextOptionsBuilder<TrackItDbContext>();

    optionsBuilder
      .UseMySql(
        "Server=localhost;Port=3306;Database=trackitservice;User=root;Password=password;SSL Mode=None;",
        new MySqlServerVersion(new Version()),
        opt => opt.EnableRetryOnFailure()
      )
      .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    
    return new TrackItDbContext(optionsBuilder.Options);
  }
}