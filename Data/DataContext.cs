using dotnet_rpg.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Data
{
  public class DataContext : DbContext
  {
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }

    // helps with saving and querying of the data from DB.
    public DbSet<Character> Characters { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Weapon> Weapons { get; set; }
    public DbSet<Skill> Skills { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Skill>().HasData(
        new Skill { Id = 1, Name = "FireBall", Damage = 50 },
        new Skill { Id = 2, Name = "Frenzy", Damage = 30 },
        new Skill { Id = 3, Name = "Blizzard", Damage = 60 }
      );
    }
  }
}