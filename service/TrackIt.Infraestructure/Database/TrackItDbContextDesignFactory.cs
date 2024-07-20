using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TrackIt.Entities.Errors;

namespace TrackIt.Infraestructure.Database;

public class TrackItDbContextDesignFactory : IDesignTimeDbContextFactory<TrackItDbContext>
{
  public TrackItDbContext CreateDbContext (string[] args)
  {
    var optionsBuilder = new DbContextOptionsBuilder<TrackItDbContext>();
    
    optionsBuilder
      .UseMySql(
        Environment.GetEnvironmentVariable("MYSQL_TRACKIT_CONNECTION_STRING"),
        new MySqlServerVersion(new Version()),
        opt => opt.EnableRetryOnFailure()
      )
      .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    
    return new TrackItDbContext(optionsBuilder.Options);
  }
}