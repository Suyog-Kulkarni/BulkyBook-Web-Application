using BulkyBookWeb.Models;
using Microsoft.EntityFrameworkCore;
namespace BulkyBookWeb.Data;
 public class ApplicationDbContext : DbContext
 {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option)
    {

    }
    // DbContext generally represents a database connection and a set of tables.
    // DbSet is used to represent a table

    public DbSet<Category> Categories { get; set; } 

    //public DbSet<Info> info { get; set; }
    //public Category CategoryCategoryCategory { get; set; }

    

}

