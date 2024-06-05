using dotnet_sample.Entities;
using Microsoft.EntityFrameworkCore;

namespace dotnet_sample.Data;

public class GameStoreContext(DbContextOptions<GameStoreContext> options) : DbContext(options) //DbContext is an object that represents a session with the database and that can be used to query and save instances of your queries
{
    public DbSet<Game> Games => Set<Game>();

    public DbSet<Genre> Genres => Set<Genre>();

    //This function is executed as soon as the migration is executed. Thus we use this opportunity to add categories to data upon migration
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genre>().HasData(
            new { Id = 1, Name = "Fighting" },
            new { Id = 2, Name = "Roleplaying" },
            new { Id = 3, Name = "Sports" },
            new { Id = 4, Name = "Racing" },
            new { Id = 5, Name = "Kids and Family" }
        );
    }
}
