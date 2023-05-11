using BulkyBook_Razor.Models;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook_Razor.Data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option)
    {
        /* reference of dbcontextoptions tell us about the configuration of connection string and database provider to use */
    }
    // DbContext generally represents a database connection and a set of tables.
    // DbSet is used to represent a table

    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    // Modelbuilder is used to configure the model for the context and to establish relationships between the models and the database 
    {
        modelBuilder.Entity<Category>().HasData(
        new Category { Id = 1, Name = "Suyog", DisplayOrder = 1 },
        new Category { Id = 2, Name = "Sui", DisplayOrder = 2 },
        new Category { Id = 3, Name = "Suii2", DisplayOrder = 3 }
        );
    }
}

