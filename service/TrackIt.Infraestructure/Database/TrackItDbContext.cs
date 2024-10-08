﻿using TrackIt.Infraestructure.Database.Mappers;
using TrackIt.Infraestructure.Security.Models;
using Microsoft.EntityFrameworkCore;
using TrackIt.Entities.Core;
using TrackIt.Entities;

namespace TrackIt.Infraestructure.Database;

public class TrackItDbContext : DbContext
{
  public static bool IsMigration { get; set; } = true;

  public DbSet<User> User { get; init; }

  public DbSet<Password> Password { get; init; }
  
  public DbSet<Ticket> Ticket { get; init; }
  
  public DbSet<RefreshToken> RefreshToken { get; init; }
  
  public DbSet<ActivityGroup> ActivityGroup { get; init; }
  
  public DbSet<Activity> Activity { get; init; }
  
  public DbSet<SubActivity> SubActivity { get; init; }
  
  public DbSet<MonthlyExpenses> MonthlyExpenses { get; init; }
  
  public DbSet<Expense> Expense { get; init; }
  
  public TrackItDbContext (DbContextOptions<TrackItDbContext> options) : base(options)
  {
  }
  
  protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder)
  {
    if (!IsMigration) return;

    optionsBuilder
      .UseMySql(
        Environment.GetEnvironmentVariable("MYSQL_TRACKIT_CONNECTION_STRING"),
        new MySqlServerVersion(new Version()),
        opt => opt.EnableRetryOnFailure()
      )
      .EnableSensitiveDataLogging();
  }
  
  protected override void OnModelCreating (ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    modelBuilder
      .Entity<User>()
      .HasOne(u => u.Password)
      .WithOne(p => p.User)
      .HasForeignKey<Password>(p => p.UserId)
      .OnDelete(DeleteBehavior.Cascade)
      .IsRequired();

    modelBuilder
      .Entity<User>()
      .HasMany<ActivityGroup>()
      .WithOne()
      .HasForeignKey(aG => aG.UserId)
      .OnDelete(DeleteBehavior.Cascade);
      
    
    modelBuilder
      .Entity<ActivityGroup>()
      .HasMany(aG => aG.Activities)
      .WithOne()
      .HasForeignKey(a => a.ActivityGroupId)
      .OnDelete(DeleteBehavior.Cascade);
    
    modelBuilder
      .Entity<Activity>()
      .HasMany(a => a.SubActivities)
      .WithOne()
      .HasForeignKey(s => s.ActivityId)
      .OnDelete(DeleteBehavior.Cascade);

    modelBuilder
      .Entity<MonthlyExpenses>()
      .HasMany(m => m.Expenses)
      .WithOne()
      .HasForeignKey(e => e.MonthlyExpensesId)
      .OnDelete(DeleteBehavior.Cascade);
    
    new UserMapper().Configure(modelBuilder.Entity<User>());
  }
}