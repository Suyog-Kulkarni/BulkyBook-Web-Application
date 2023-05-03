﻿using BulkyBookWeb.Data;
using BulkyBookWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

// while working with controllers restart to application to see the changes


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
        // ONE CREATE TO SHOW TO VIEW AND ANOTHER TO SEND DATA TO DATABASE 
        
        // POST
        /* Post method is mainly used at the client(Browser) side to send data to a Specified 
         server in order to create or rewrite a particular resource/data.*/
        [HttpPost] 
        [ValidateAntiForgeryToken]

        public IActionResult Create(Category obj)
        {
            // this method didnt run after pressing createnewcat. because we defined it as post(send data method)
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "Name and DisplayOrder cannot be same.");
                // will be displayed only under name else use Customerror to display overall using 
                // all in asp-validation-summary helper tag in view
            }
            if (ModelState.IsValid)// modelstate.isvalid checks that is it possible to bind the model with the view
            {

                _db.Categories.Add(obj);
                _db.SaveChanges();
                TempData["Success"] = "Category created successfully";
                return Redirect("Index");
            }

            return View(obj);// to stay on same page returning the same object or else return only view



        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryfromDb = _db.Categories.Find(id);// retrives the data from database by using id
/*
            var category = _db.Categories.FirstOrDefault(u => u.Id == id);
            var categorys = _db.Categories.SingleOrDefault(u => u.Id == id);*/
            if (categoryfromDb is null)
            {
                return NotFound();
            }
            return View(categoryfromDb);

        }
        

        // POST
        /* Post method is mainly used at the client(Browser) side to send data to a Specified 
         server in order to create or rewrite a particular resource/data.*/
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Edit(Category obj) // FIND HOW THIS IS CONNECTED TO CREATE BUTTON 
         // because it is a post method and we also defined post inside the view
        {
            // this method didnt run after pressing createnewcat. because we defined it as post(send data method)
            /*if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "Name and Display Order cannot be same.");
                // will be displayed only under name else use Customerror to display overall using 
                // all in asp-validation-summary helper tag in view
            }*/
            if (ModelState.IsValid)
            {
                _db.Categories.Update(obj);
                _db.SaveChanges();
                TempData["Success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }
               return View(obj);// to stay on same page returning the same object or else return only view
        }
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryfromDb = _db.Categories.Find(id);// retrives the data from database by using id
            /*
                        var category = _db.Categories.FirstOrDefault(u => u.Id == id);
                        var categorys = _db.Categories.SingleOrDefault(u => u.Id == id);*/
            if (categoryfromDb is null)
            {
                return NotFound();
            }
            return View(categoryfromDb);
        }
        [HttpPost,ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            var item = _db.Categories.Find(id);
            if(item is null)
            {
                return NotFound();
            }
            _db.Categories.Remove(item);
            _db.SaveChanges();
            TempData["Success"] = "Category deleted successfully";
            return RedirectToAction("Index");
            /*
             by getting obj use
            _db.cat.remove(obj);
            */

        }

    }
}
