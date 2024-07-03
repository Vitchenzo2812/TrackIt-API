using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace TrackIt.Building.Contracts;

public interface IStartup
{
  void ConfigureServices (IServiceCollection services);

  void ConfigureDbContext (IServiceCollection services);

  void MigrateDatabase (IApplicationBuilder app);
}