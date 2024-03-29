﻿// <auto-generated />
using System;
using Everest.AspNetStartup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Everest.AspNetStartup.Migrations
{
    [DbContext(typeof(PersistenceContext))]
    partial class PersistenceContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Everest.AspNetStartup.Entities.Connection", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccessToken");

                    b.Property<DateTime?>("BeginDate");

                    b.Property<string>("Browser");

                    b.Property<DateTime?>("EndDate");

                    b.Property<bool>("IsPersistent");

                    b.Property<string>("OS");

                    b.Property<string>("RefreshToken");

                    b.Property<DateTime>("RegistrationDate");

                    b.Property<string>("RemoteAddress");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Connections");
                });

            modelBuilder.Entity("Everest.AspNetStartup.Entities.Role", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<DateTime>("RegistrationDate");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Everest.AspNetStartup.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("APIURL");

                    b.Property<string>("AboutMe");

                    b.Property<DateTime>("BirthDate");

                    b.Property<string>("City");

                    b.Property<string>("Country");

                    b.Property<string>("Email");

                    b.Property<string>("Gender");

                    b.Property<string>("ImageName");

                    b.Property<string>("ImageURL");

                    b.Property<string>("Name");

                    b.Property<string>("NationalId");

                    b.Property<string>("Password");

                    b.Property<string>("PhoneNumber");

                    b.Property<string>("PostalCode");

                    b.Property<DateTime>("RegistrationDate");

                    b.Property<string>("ResetPasswordCode");

                    b.Property<DateTime>("ResetPasswordCodeCreateTime");

                    b.Property<string>("State");

                    b.Property<string>("Street");

                    b.Property<string>("Surname");

                    b.Property<string>("Username");

                    b.Property<string>("WebURL");

                    b.Property<string>("Website");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Everest.AspNetStartup.Entities.UserRole", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("RegistrationDate");

                    b.Property<string>("RoleId");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("Everest.AspNetStartup.Entities.Connection", b =>
                {
                    b.HasOne("Everest.AspNetStartup.Entities.User", "User")
                        .WithMany("Connections")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Everest.AspNetStartup.Entities.UserRole", b =>
                {
                    b.HasOne("Everest.AspNetStartup.Entities.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId");

                    b.HasOne("Everest.AspNetStartup.Entities.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}
