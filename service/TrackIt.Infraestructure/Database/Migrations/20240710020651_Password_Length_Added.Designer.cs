﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TrackIt.Infraestructure.Database;

#nullable disable

namespace TrackIt.Infraestructure.Database.Migrations
{
    [DbContext(typeof(TrackItDbContext))]
    [Migration("20240710020651_Password_Length_Added")]
    partial class Password_Length_Added
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("TrackIt.Entities.Password", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Hash")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("PasswordLength")
                        .HasColumnType("int");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Password");
                });

            modelBuilder.Entity("TrackIt.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Email")
                        .HasColumnType("longtext")
                        .HasColumnName("Email");

                    b.Property<double?>("Income")
                        .HasColumnType("double");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<Guid?>("PasswordId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("PasswordId");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("TrackIt.Entities.User", b =>
                {
                    b.HasOne("TrackIt.Entities.Password", "Password")
                        .WithMany()
                        .HasForeignKey("PasswordId");

                    b.Navigation("Password");
                });
#pragma warning restore 612, 618
        }
    }
}
