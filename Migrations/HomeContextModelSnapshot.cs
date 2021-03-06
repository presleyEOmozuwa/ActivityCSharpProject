﻿// <auto-generated />
using System;
using ActivityCenter.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ActivityCenter.Migrations
{
    [DbContext(typeof(HomeContext))]
    partial class HomeContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ActivityCenter.Models.Association", b =>
                {
                    b.Property<int>("AssociationId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("GameId");

                    b.Property<int>("UserId");

                    b.HasKey("AssociationId");

                    b.HasIndex("GameId");

                    b.HasIndex("UserId");

                    b.ToTable("Associations");
                });

            modelBuilder.Entity("ActivityCenter.Models.Game", b =>
                {
                    b.Property<int>("GameId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<DateTime>("GameDate");

                    b.Property<DateTime>("Time");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<DateTime>("UpdatedAt");

                    b.Property<int>("UserId");

                    b.HasKey("GameId");

                    b.HasIndex("UserId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("ActivityCenter.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ActivityCenter.Models.Association", b =>
                {
                    b.HasOne("ActivityCenter.Models.Game", "Game")
                        .WithMany("Participants")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ActivityCenter.Models.User", "User")
                        .WithMany("Games")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ActivityCenter.Models.Game", b =>
                {
                    b.HasOne("ActivityCenter.Models.User", "Creator")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
