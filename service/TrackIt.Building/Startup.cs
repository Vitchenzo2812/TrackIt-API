using TrackIt.Commands.ActivityGroupCommands.CreateActivityGroup;
using TrackIt.Commands.ActivityGroupCommands.UpdateActivityGroup;
using TrackIt.Commands.ActivityGroupCommands.DeleteActivityGroup;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TrackIt.Commands.ActivityCommands.CreateActivity;
using TrackIt.Commands.ActivityCommands.UpdateActivity;
using TrackIt.Commands.ActivityCommands.DeleteActivity;
using TrackIt.Infraestructure.Database.Interceptor;
using TrackIt.Commands.UserCommands.UpdatePassword;
using TrackIt.Infraestructure.Security.Contracts;
using TrackIt.Infraestructure.Database.Contracts;
using Microsoft.Extensions.DependencyInjection;
using TrackIt.Infraestructure.Mailer.Contracts;
using TrackIt.Commands.UserCommands.DeleteUser;
using TrackIt.Commands.UserCommands.UpdateUser;
using TrackIt.Commands.Auth.ForgotPassword;
using TrackIt.Infraestructure.Repository;
using TrackIt.Infraestructure.Database;
using TrackIt.Infraestructure.Security;
using TrackIt.Infraestructure.Mailer;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using TrackIt.Commands.Auth.SignUp;
using TrackIt.Building.Contracts;
using TrackIt.Events.Consumers;
using TrackIt.Queries.GetUsers;
using TrackIt.Queries.GetUser;
using TrackIt.Queries.Views;
using MassTransit;
using MediatR;
using TrackIt.Commands.ExpenseCommands.CreateExpense;
using TrackIt.Commands.ExpenseCommands.DeleteExpense;
using TrackIt.Commands.ExpenseCommands.UpdateExpense;
using TrackIt.Commands.MonthlyExpensesCommands.DeleteMonthlyExpenses;
using TrackIt.Commands.MonthlyExpensesCommands.UpdateMonthlyExpenses;
using TrackIt.Commands.SubActivityCommands.CreateSubActivity;
using TrackIt.Commands.SubActivityCommands.DeleteSubActivity;
using TrackIt.Commands.SubActivityCommands.UpdateSubActivity;
using TrackIt.Entities.Repository;
using TrackIt.Entities.Services;

namespace TrackIt.Building;

public abstract class TrackItStartup : IStartup
{
  public abstract void MigrateDatabase (IApplicationBuilder app);
  
  public virtual void ConfigureServices (IServiceCollection services)
  {
    ConfigureDbContext(services);
    ConfigureMassTransit(services);
    
    services.TryAddSingleton<PublishEvents>();
    services.AddTransient<MonthlyExpensesService>();
    services.AddTransient<IJwtService, JwtService>();
    services.AddTransient<IUnitOfWork, UnitOfWork>();
    services.AddTransient<IMailerService, MailerService>();
    services.AddTransient<ISessionService, SessionService>();
    services.AddTransient<IUserRepository, UserRepository>();
    services.AddTransient<ITicketRepository, TicketRepository>();
    services.AddTransient<IExpenseRepository, ExpenseRepository>();
    services.AddTransient<ICategoryRepository, CategoryRepository>();
    services.AddTransient<IActivityRepository, ActivityRepository>();
    services.AddTransient<IRefreshTokenService, RefreshTokenService>();
    services.AddTransient<ISubActivityRepository, SubActivityRepository>();
    services.AddTransient<IPaymentFormatRepository, PaymentFormatRepository>();
    services.AddTransient<IActivityGroupRepository, ActivityGroupRepository>();
    services.AddTransient<IMonthlyExpensesRepository, MonthlyExpensesRepository>();
    
    services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(SignUpCommand)));
    services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(GetUserQuery)));

    services.AddTransient<IPipelineBehavior<GetUserQuery, UserView>, GetUserRealmHandle>();
    services.AddTransient<IPipelineBehavior<GetUsersQuery, PaginationView<List<UserResourceView>>>, GetUsersRealmHandle>();
    services.AddTransient<IPipelineBehavior<UpdateUserCommand, Unit>, UpdateUserRealmHandle>();
    services.AddTransient<IPipelineBehavior<DeleteUserCommand, Unit>, DeleteUserRealmHandle>();
    services.AddTransient<IPipelineBehavior<UpdatePasswordCommand, Unit>, UpdatePasswordRealmHandle>();
    services.AddTransient<IPipelineBehavior<ForgotPasswordCommand, ForgotPasswordResponse>, ForgotPasswordRealmHandle>();
    
    services.AddTransient<IPipelineBehavior<CreateActivityGroupCommand, Unit>, CreateActivityGroupRealmHandle>();
    services.AddTransient<IPipelineBehavior<UpdateActivityGroupCommand, Unit>, UpdateActivityGroupRealmHandle>();
    services.AddTransient<IPipelineBehavior<DeleteActivityGroupCommand, Unit>, DeleteActivityGroupRealmHandle>();
    
    services.AddTransient<IPipelineBehavior<CreateActivityCommand, Unit>, CreateActivityRealmHandle>();
    services.AddTransient<IPipelineBehavior<UpdateActivityCommand, Unit>, UpdateActivityRealmHandle>();
    services.AddTransient<IPipelineBehavior<DeleteActivityCommand, Unit>, DeleteActivityRealmHandle>();
    
    services.AddTransient<IPipelineBehavior<CreateSubActivityCommand, Unit>, CreateSubActivityRealmHandle>();
    services.AddTransient<IPipelineBehavior<UpdateSubActivityCommand, Unit>, UpdateSubActivityRealmHandle>();
    services.AddTransient<IPipelineBehavior<DeleteSubActivityCommand, Unit>, DeleteSubActivityRealmHandle>();
    
    services.AddTransient<IPipelineBehavior<CreateExpenseCommand, Unit>, CreateExpenseRealmHandle>();
    services.AddTransient<IPipelineBehavior<UpdateExpenseCommand, Unit>, UpdateExpenseRealmHandle>();
    services.AddTransient<IPipelineBehavior<DeleteExpenseCommand, Unit>, DeleteExpenseRealmHandle>();
    
    services.AddTransient<IPipelineBehavior<UpdateMonthlyExpensesCommand, Unit>, UpdateMonthlyExpensesRealmHandle>();
    services.AddTransient<IPipelineBehavior<DeleteMonthlyExpensesCommand, Unit>, DeleteMonthlyExpensesRealmHandle>();
  }

  public void ConfigureMassTransit (IServiceCollection services)
  {
    if (Environment.GetEnvironmentVariable("Environment") == "Tests") return;

    services.AddMassTransit(x =>
    {
      x.AddConsumer<SendEmailAboutSignUpConsumer>();
      x.AddConsumer<EmailForgotPasswordConsumer>();
      
      x.UsingRabbitMq((ctx, cfg) =>
      {
        cfg.Host(Environment.GetEnvironmentVariable("RABBITMQ_HOSTNAME"), "/", h =>
        {
          h.Username(Environment.GetEnvironmentVariable("RABBITMQ_USERNAME")!);
          h.Password(Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD")!);
        });
        
        cfg.ConfigureEndpoints(ctx);
        cfg.UseConcurrencyLimit(1);
      });
    });
  }

  public void ConfigureDbContext (IServiceCollection services)
  {
    services.AddDbContext<TrackItDbContext>((sp, options) =>
    {
      options
        .AddInterceptors(sp.GetRequiredService<PublishEvents>())
        .UseMySql(
          Environment.GetEnvironmentVariable("MYSQL_TRACKIT_CONNECTION_STRING"),
          new MySqlServerVersion(new Version())
        )
        .EnableSensitiveDataLogging()
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    });
  }
}