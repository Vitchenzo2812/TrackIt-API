using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Infraestructure.Security.Contracts;
using TrackIt.Infraestructure.Database.Contracts;
using Microsoft.Extensions.DependencyInjection;
using TrackIt.Infraestructure.Repository;
using TrackIt.Infraestructure.Database;
using TrackIt.Infraestructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using TrackIt.Commands.Auth.SignUp;
using TrackIt.Commands.DeleteUser;
using TrackIt.Commands.UpdateUser;
using TrackIt.Building.Contracts;
using TrackIt.Entities.Errors;
using TrackIt.Queries.GetUser;
using TrackIt.Queries.Views;
using MediatR;

namespace TrackIt.Building;

public abstract class TrackItStartup : IStartup
{
  public abstract void MigrateDatabase (IApplicationBuilder app);
  
  public virtual void ConfigureServices (IServiceCollection services)
  {
    ConfigureDbContext(services);
    
    services.AddTransient<IJwtService, JwtService>();
    services.AddTransient<IUserRepository, UserRepository>();
    services.AddTransient<IUnitOfWork, UnitOfWork>();
    
    services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(SignUpCommand)));
    services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(GetUserQuery)));

    services.AddTransient<IPipelineBehavior<GetUserQuery, UserView>, GetUserRealmHandle>();
    services.AddTransient<IPipelineBehavior<UpdateUserCommand, Unit>, UpdateUserRealmHandle>();
    services.AddTransient<IPipelineBehavior<DeleteUserCommand, Unit>, DeleteUserRealmHandle>();
  }
  
  public void ConfigureDbContext (IServiceCollection services)
  {
    TrackItDbContext.IsMigration = false;
    
    var connection =
      Environment.GetEnvironmentVariable("MYSQL_TRACKIT_CONNECTION_STRING");

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
  }
}