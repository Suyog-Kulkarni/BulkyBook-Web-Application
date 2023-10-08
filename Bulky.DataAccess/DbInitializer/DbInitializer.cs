using Bulky.DataAccess.Data;
using BulkyBook.Models;
using BulkyBook.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.DbInitializer;
public class DbInitializer : IDbInitializer
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _db;

    public DbInitializer(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext db)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _db = db;
    }
    public void Initialize()
    {
        // migration if they are not applied

        try
        {
            if (_db.Database.GetPendingMigrations().Count() > 0)
            {
                _db.Database.Migrate();
            }
        }
        catch(Exception ex)
        {}

        // roles if they are not created

        if (!_roleManager.RoleExistsAsync(SD.Role_Customer).GetAwaiter().GetResult())
        {
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Company)).GetAwaiter().GetResult();
            /*await*/
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();

            // if roles are not created, then we will create new admin user as well
            _userManager.CreateAsync
             (
                new ApplicationUser
                {
                    Name = "Suyog Kulkarni",
                    UserName = "kulkarnisuyog192@gmail.com",
                    Email = "kulkarnisuyog192@gmail.com",
                    PhoneNumber = "9175445552",
                    StreetAddress = "KJ Hostel",
                    State = "Maha",
                    PostalCode = "411048",
                    City = "Pune"
                }, "Admin@123")
             .GetAwaiter().GetResult();

            ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "kulkarnisuyog192@gmail.com");
            _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();

        }
        return;
    }
}


