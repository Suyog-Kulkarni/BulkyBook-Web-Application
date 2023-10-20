using Bulky.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModel;
using BulkyBook.Utilities;
using MessagePack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public UserController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id is null or 0)
            {
                return NotFound();
            }

            var userFromDb = _applicationDbContext.ApplicationUsers.Find(id);

            if(userFromDb is null)
            {
                return NotFound();
            }

            return View(userFromDb);
        }

        #region API CALLS
        [HttpGet]

        public IActionResult GetAll()
        {
            List<ApplicationUser> applicationUsers = _applicationDbContext.ApplicationUsers.Include(u => u.Company).ToList();
            // above statement exculdes the user that is not in the company so i changed the company to nullable in the ApplicationUser.cs
            var userRoles = _applicationDbContext.UserRoles.ToList();
            var roles = _applicationDbContext.Roles.ToList();

            
            
            foreach(var user in applicationUsers)
            {
                var RolesId = userRoles.FirstOrDefault(u => u.UserId == user.Id).RoleId;

                user.Role = roles.FirstOrDefault( u=> u.Id == RolesId).Name;
                 
                if(user.Company is null)
                {
                    user.Company = new() {Name = "" };
                }
            }// and here i am adding the company object to the user that is not in the company so that i can display the user in the view 

            return Json(new { data = applicationUsers });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            
            return Json(new { success = true, message = "Delete Successful" });

        }
        #endregion
    }
}
