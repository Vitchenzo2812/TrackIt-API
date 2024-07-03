using Microsoft.EntityFrameworkCore.Design;
using TrackIt.Infraestructure.Extensions;
using TrackIt.Infraestructure.Config;
using Microsoft.EntityFrameworkCore;

namespace TrackIt.Infraestructure.Database;

public class TrackItDbContextDesignFactory : IDesignTimeDbContextFactory<TrackItDbContext>
{
  public TrackItDbContext CreateDbContext (string[] args)
  {
    var optionsBuilder = new DbContextOptionsBuilder<TrackItDbContext>();

    optionsBuilder
      .UseMySql(
        Environment.GetEnvironmentVariable(EnvironmentVariables.MySqlTrackItConnectionString.Description()),
        new MySqlServerVersion(new Version())
      )
      .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

    return new TrackItDbContext(optionsBuilder.Options);
  }
}