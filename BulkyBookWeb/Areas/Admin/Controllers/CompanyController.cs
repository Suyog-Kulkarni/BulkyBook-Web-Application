using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModel;
using BulkyBook.Utilities;
using MessagePack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Data;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Company> Companys = _unitOfWork.Company.GetAll().ToList();
            return View(Companys);
        }
        // GET 
        [HttpGet]
        public IActionResult Upsert(int? id)// upsert is a combination of update and insert 
        {
            /*IEnumerable<SelectListItem> listItems = _unitOfWork.Category.GetAll().Select( // this is a list of select list items 
                u => new SelectListItem// this is a class that represents an item in a select list 
                {// selectlistitem is not an anonymous type because it is already defined class in asp.net 
                    Text = u.Name,
                    Value = u.Id.ToString()
                }
             );*/// this concept is called projection in which we project the data from one form to another  
           // ViewBag.CategoryList = listItems;// this is a dynamic property that can be used to pass data from the controller to the view 
            // and we use projection because we want to pass only the name and id of the category to the view and not the entire category object
            /*CompanyVM CompanyVM = new()// viewmodel
            {
                Company = new Company(),
                CategoryList = listItems
            };*/// this is an object of the CompanyVM class that we created in the models folder
            if(id is null or 0)
            {
                //create
                return View(new Company());

            }
            else
            {
                //update
                Company CompanyObj = _unitOfWork.Company.Get(u => u.Id == id);
                return View(CompanyObj);
            }
            

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company obj)
        {
            if (ModelState.IsValid)
            {

                    if(obj.Id is 0)
                    {
                        // to create a new Company
                        _unitOfWork.Company.Add(obj);
                    }
                    else
                    {
                        // to update an existing Company
                        _unitOfWork.Company.Update(obj);
                    }
                

                
                _unitOfWork.Save();
                TempData["Success"] = "Company created successfully";
                return RedirectToAction(nameof(Index));// or return View("Index"); or rediect("Index");

            }
            else 
            // even if the model state is not valid the categorylist must be populated with the data that the user enteredz 
            {// if the model state is not valid then we need to return the view with the data that the user entered 
                /*vM.CategoryList = _unitOfWork.Category.GetAll().Select(
                    u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    });*/
                return View(obj);
            }
            
        }
        /*[HttpGet]
        public IActionResult Edit(int? id)
        {
            if(id is null or 0)
            {
                return NotFound();
            }
            var Companyfromdb = _unitOfWork.Company.Get(p => p.Id == id);

            if(Companyfromdb is null)
            {
                return NotFound();
            }
            return View(Companyfromdb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Edit(Company Company)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Company.Update(Company);
                _unitOfWork.Save();
                TempData["Success"] = "Company Updated successfully";
                return RedirectToAction(nameof(Index));// or return View("Index"); or rediect("Index");

            }
            return View(Company);

        }*/
        /*[HttpGet]
        public IActionResult Delete(int? id)
        {
            if(id is null or 0)
            {
                return NotFound();
            }
            var Companyfromdb = _unitOfWork.Company.Get(p => p.Id == id);

            if(Companyfromdb is null)
            {
                return NotFound();
            }
            return View(Companyfromdb);
        }

        [HttpPost,ActionName("Delete")]
        public IActionResult DeleteProd(int? id)
        {
            var Company = _unitOfWork.Company.Get(p => p.Id == id);
            if(Company is null)
            {
                return NotFound();
            }
            _unitOfWork.Company.Remove(Company);
            _unitOfWork.Save();
            TempData["Success"] = "Company Deleted successfully";
            return RedirectToAction(nameof(Index));

        }*/
        #region API CALLS
        [HttpGet]

        public IActionResult GetAll()
        {
            List<Company> Companys = _unitOfWork.Company.GetAll().ToList();

            return Json(new { data = Companys });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var CompanyToBeDeleted = _unitOfWork.Company.Get(u => u.Id == id);

            if(CompanyToBeDeleted is null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Company.Remove(CompanyToBeDeleted);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });

        }
        #endregion
    }
}
