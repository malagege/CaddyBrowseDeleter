using Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CaddyBrowseDeleter;

public class AppDbContext : DbContext
{
    public DbSet<ToDoDeleteFile> ToDoDeleteFiles { get; set; }
    public DbSet<User> Users { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var dbDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "db");
        if (!Directory.Exists(dbDirectory))
        {
            Directory.CreateDirectory(dbDirectory);
        }

        optionsBuilder.UseSqlite($"Data Source={Path.Combine(dbDirectory, "app.db")}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Name = "malagege"},
            new User { Id = 2, Name = "chevy"}
        );
    }
}