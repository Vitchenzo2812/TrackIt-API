using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TrackIt.Entities;

namespace TrackIt.Infraestructure.Database.Mappers;

public class UserMapper : IEntityTypeConfiguration<User>
{
  public void Configure (EntityTypeBuilder<User> builder)
  {
    builder.ToTable("User");

    builder.HasKey(u => u.Id);

    builder
      .Property(u => u.Email)
      .HasColumnName("Email")
      .HasConversion(
        email => email.Value,
        value => Email.FromAddress(value)
      );
  }
}