using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using TrackIt.Infraestructure.Database.Contracts;
using Microsoft.Extensions.DependencyInjection;
using TrackIt.Infraestructure.Extensions;
using TrackIt.Infraestructure.Database;
using TrackIt.Infraestructure.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using TrackIt.Building.Contracts;

namespace TrackIt.Building;

public abstract class TrackItStartup : IStartup
{
  public abstract void MigrateDatabase (IApplicationBuilder app);
  
  public virtual void ConfigureServices (IServiceCollection services)
  {
  }
  
  public void ConfigureDbContext (IServiceCollection services)
  {
    services.AddDbContext<TrackItDbContext>(options =>
    {
      options
        .UseMySql(
          Environment.GetEnvironmentVariable(EnvironmentVariables.MySqlTrackItConnectionString.Description()),
          new MySqlServerVersion(new Version()),
          opt => opt.MigrationsAssembly(typeof(TrackItDbContext).Assembly.FullName)
            .SchemaBehavior(MySqlSchemaBehavior.Translate, (schema, entity) => $"{schema}.{entity}")
        );
    });

    services.AddTransient<IUnitOfWork, UnitOfWork>();
  }
}