using Microsoft.AspNetCore.Authentication.JwtBearer;
using TrackIt.Infraestructure.Web.Middlewares;
using TrackIt.Infraestructure.Web.Swagger;
using TrackIt.Infraestructure.Database;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TrackIt.Building;
using System.Text;

namespace TrackIt.WebApi;

public class WebApiTrackItStartup : TrackItStartup
{
  public override void ConfigureServices (IServiceCollection services)
  {
    base.ConfigureServices(services);
    
    services
      .AddControllers()
      .AddJsonOptions(opt => opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
    
    services.AddSwaggerGen(c =>
    {
      c.SwaggerDoc(
        "trackIt",
        new OpenApiInfo
        {
          Title = "TrackIt Api",
          Version = "v1"
        }
      );
    });

    services.AddCors(opt =>
    {
      opt.AddDefaultPolicy(policy =>
      {
        policy
          .AllowAnyHeader()
          .AllowAnyOrigin()
          .AllowAnyMethod();
      });
    });
    
    var secret = Environment.GetEnvironmentVariable("JWT_SECRET");

    services
      .AddAuthentication(x =>
      {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      })
      .AddJwtBearer(x =>
      {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret!)),
          ValidateIssuer = false,
          ValidateAudience = false
        };
      });

    services.AddSwaggerGen(opt => opt.AddJWTAuth());
  }
 
  public override void MigrateDatabase (IApplicationBuilder app)
  {
    var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<TrackItDbContext>();
    dbContext.Database.Migrate();
  }
  
  public void Configure (IApplicationBuilder app, IWebHostEnvironment env)
  {
    if (!env.IsEnvironment("Tests"))
      MigrateDatabase(app);
    
    if (env.IsDevelopment())
      app.UseDeveloperExceptionPage();
      
    app.UseCors();
    
    app.UseMiddleware<GlobalExceptionMiddleware>();
    
    app.UseSwagger();
    app.UseSwaggerUI(config =>
    {
      config.SwaggerEndpoint("/swagger/trackIt/swagger.json", "TrackIt API");
      
      config.RoutePrefix = "docs";
      config.InjectStylesheet("/wwwroot/swagger-ui/SwaggerDark.css");
    });
    
    app.UseRouting();
    
    app.UseMiddleware<AuthorizationMiddleware>();
    
    app.UseEndpoints(endpoints => endpoints.MapControllers());
      
    app.UseHttpsRedirection();
  }
}