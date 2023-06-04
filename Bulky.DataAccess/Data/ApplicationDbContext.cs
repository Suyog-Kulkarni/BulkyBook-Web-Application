using Bulky.Models;
using Microsoft.EntityFrameworkCore;
namespace Bulky.DataAccess.Data;
public class ApplicationDbContext : DbContext
 {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option)
    {
        /* reference of dbcontextoptions tell us about the configuration of connection string and database provider to use */
    }
    // DbContext generally represents a database connection and a set of tables.
    // DbSet is used to represent a table

    public DbSet<Category> Categories { get; set; } 

    //public DbSet<Info> info { get; set; }
    //public Category CategoryCategoryCategory { get; set; }
    

}

