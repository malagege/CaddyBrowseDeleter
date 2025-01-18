﻿// <auto-generated />
using CaddyBrowseDeleter;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CaddyBrowseDeleter.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250118154759_ToDoDeleteManyToMany")]
    partial class ToDoDeleteManyToMany
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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
