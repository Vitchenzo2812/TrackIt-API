﻿using TrackIt.Infraestructure.Database.Mappers;
using TrackIt.Infraestructure.Security.Models;
using Microsoft.EntityFrameworkCore;
using TrackIt.Entities.Core;
using TrackIt.Entities;
using TrackIt.Entities.Activities;
using TrackIt.Entities.Expenses;

namespace TrackIt.Infraestructure.Database;

public class TrackItDbContext : DbContext
{
  public DbSet<User> User { get; init; }
  public DbSet<Password> Password { get; init; }
  public DbSet<Ticket> Ticket { get; init; }
  public DbSet<RefreshToken> RefreshToken { get; init; }
  public DbSet<Activity> Activities { get; set; }
  public DbSet<SubActivity> SubActivities { get; set; }
  public DbSet<ActivityGroup> ActivityGroups { get; set; }
  public DbSet<Expense> Expenses { get; set; }
  public DbSet<MonthlyExpenses> MonthlyExpenses { get; set; }
  public DbSet<PaymentFormat> PaymentFormats { get; set; }
  public DbSet<PaymentFormatConfig> PaymentFormatConfigs { get; set; }
  public DbSet<Category> Categories { get; set; }
  public DbSet<CategoryConfig> CategoryConfigs { get; set; }
  
  public TrackItDbContext (DbContextOptions<TrackItDbContext> options) : base(options)
  {
  }
  
  protected override void OnModelCreating (ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    modelBuilder
      .Entity<User>()
      .HasOne(x => x.Password)
      .WithOne(x => x.User)
      .HasForeignKey<Password>(x => x.UserId)
      .OnDelete(DeleteBehavior.Cascade)
      .IsRequired();

    modelBuilder
      .Entity<ActivityGroup>()
      .HasMany(x => x.Activities)
      .WithOne()
      .HasForeignKey(x => x.ActivityGroupId)
      .OnDelete(DeleteBehavior.Cascade);

    modelBuilder
      .Entity<Activity>()
      .HasMany(x => x.SubActivities)
      .WithOne()
      .HasForeignKey(x => x.ActivityId)
      .OnDelete(DeleteBehavior.Cascade);

    modelBuilder
      .Entity<MonthlyExpenses>()
      .HasMany<Expense>()
      .WithOne()
      .HasForeignKey(x => x.MonthlyExpensesId)
      .OnDelete(DeleteBehavior.Cascade);

    modelBuilder
      .Entity<Expense>()
      .HasOne<PaymentFormat>()
      .WithMany()
      .HasForeignKey(x => x.PaymentFormatId);
    
    modelBuilder
      .Entity<Expense>()
      .HasOne<Category>()
      .WithMany()
      .HasForeignKey(x => x.CategoryId);

    modelBuilder
      .Entity<PaymentFormat>()
      .HasOne(x => x.PaymentFormatConfig)
      .WithOne(x => x.PaymentFormat)
      .HasForeignKey<PaymentFormatConfig>(x => x.PaymentFormatId)
      .OnDelete(DeleteBehavior.Cascade);
    
    modelBuilder
      .Entity<Category>()
      .HasOne(x => x.CategoryConfig)
      .WithOne(x => x.Category)
      .HasForeignKey<CategoryConfig>(x => x.CategoryId)
      .OnDelete(DeleteBehavior.Cascade);
    
    new UserMapper().Configure(modelBuilder.Entity<User>());
  }
}