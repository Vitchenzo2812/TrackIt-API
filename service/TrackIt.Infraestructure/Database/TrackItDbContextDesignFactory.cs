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
        Environment.GetEnvironmentVariable("MYSQL_TRACKIT_CONNECTION_STRING")!,
        new MySqlServerVersion(new Version())
      )
      .EnableSensitiveDataLogging()
      .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    
    return new TrackItDbContext(optionsBuilder.Options);
  }
}