using Microsoft.Extensions.DependencyInjection.Extensions;
using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Infraestructure.Security.Contracts;
using TrackIt.Infraestructure.Database.Contracts;
using Microsoft.Extensions.DependencyInjection;
using TrackIt.Infraestructure.Repository;
using TrackIt.Infraestructure.Database;
using TrackIt.Infraestructure.Security;
using Microsoft.EntityFrameworkCore;
using TrackIt.Commands.Auth.SignUp;
using Microsoft.AspNetCore.Builder;
using TrackIt.Building.Contracts;
using TrackIt.Entities.Errors;
using TrackIt.Infraestructure.Config;
using TrackIt.Infraestructure.Extensions;

namespace TrackIt.Building;

public abstract class TrackItStartup : IStartup
{
  public abstract void MigrateDatabase (IApplicationBuilder app);
  
  public virtual void ConfigureServices (IServiceCollection services)
  {
    services.TryAddTransient<IJwtService, JwtService>();
    
    services.AddTransient<IUserRepository, UserRepository>();

    services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(SignUpCommand)));
  }
  
  public void ConfigureDbContext (IServiceCollection services)
  {
    TrackItDbContext.IsMigration = false;
    
    var connection =
      Environment.GetEnvironmentVariable(EnvironmentVariables.MySqlTrackItConnectionString.Description());

    if (string.IsNullOrEmpty(connection))
      throw new InternalServerError("Connection string not found");
    
    services.AddDbContext<TrackItDbContext>(options =>
    {
      options
        .UseMySql(
          connection,
          new MySqlServerVersion(new Version())
        );
    });

    services.TryAddTransient<IUnitOfWork, UnitOfWork>();
  }
}