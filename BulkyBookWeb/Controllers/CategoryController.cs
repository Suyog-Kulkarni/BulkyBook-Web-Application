﻿using BulkyBookWeb.Data;
using BulkyBookWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace BulkyBookWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _db.Categories;

           // IEnumerable<Info> item = _db.info;

           
            
            return View(objCategoryList);
          
        }
        //GET
        /*Get method is mainly used at the client (Browser) side to send a request to a specified
        server to get certain data or resources.*/
        public IActionResult Create()
        {
            return View();

        }
        public IActionResult Sample()
        {
           
            return View ();

        }
        
        // POST
        /* Post method is mainly used at the client(Browser) side to send data to a Specified 
         server in order to create or rewrite a particular resource/data.*/
        [HttpPost] 
        [ValidateAntiForgeryToken]

        public IActionResult Create(Category obj) // FIND HOW THIS IS CONNECTED TO CREATE BUTTON 
        {
            // this method didnt run after pressing createnewcat. because we defined it as post(send data method)
            
                
                _db.Categories.Add(obj);
                _db.SaveChanges();
                return Redirect("Index");
            
            /*return Redirect("Index");*/
            

        }
       

    }
}
