﻿// <auto-generated />
using CaddyBrowseDeleter;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CaddyBrowseDeleter.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.0");

            modelBuilder.Entity("Data.ToDoDeleteFile", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("DirPath")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ToDoDeleteFiles");
                });

            modelBuilder.Entity("Data.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Name = "malagege"
                        },
                        new
                        {
                            Id = 2L,
                            Name = "chevy"
                        });
                });

            modelBuilder.Entity("ToDoDeleteFileUser", b =>
                {
                    b.Property<long>("ToDoDeleteFilesId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("UsersId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ToDoDeleteFilesId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("ToDoDeleteFileUser");
                });

            modelBuilder.Entity("ToDoDeleteFileUser", b =>
                {
                    b.HasOne("Data.ToDoDeleteFile", null)
                        .WithMany()
                        .HasForeignKey("ToDoDeleteFilesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
