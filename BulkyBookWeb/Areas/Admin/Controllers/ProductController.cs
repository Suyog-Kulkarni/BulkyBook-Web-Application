using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModel;
using MessagePack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> products = _unitOfWork.Product.GetAll(includeProp:"Category").ToList();
            return View(products);
        }
        // GET 
        [HttpGet]
        public IActionResult Upsert(int? id)// upsert is a combination of update and insert 
        {
            IEnumerable<SelectListItem> listItems = _unitOfWork.Category.GetAll().Select( // this is a list of select list items 
                u => new SelectListItem// this is a class that represents an item in a select list 
                {// selectlistitem is not an anonymous type because it is already defined class in asp.net 
                    Text = u.Name,
                    Value = u.Id.ToString()
                }
             );// this concept is called projection in which we project the data from one form to another  
           // ViewBag.CategoryList = listItems;// this is a dynamic property that can be used to pass data from the controller to the view 
            // and we use projection because we want to pass only the name and id of the category to the view and not the entire category object
            ProductVM productVM = new()// viewmodel
            {
                Product = new Product(),
                CategoryList = listItems
            };// this is an object of the productVM class that we created in the models folder
            if(id is null or 0)
            {
                //create
                return View(productVM);

            }
            else
            {
                //update
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);
            }
            

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM vM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _hostEnvironment.WebRootPath;

                if (file is not null)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string ProductPath = Path.Combine(webRootPath, @"images\product");

                    if (!string.IsNullOrEmpty(vM.Product.ImageUrl))
                    {
                        //delete the original file
                        string oldfilepath = Path.Combine(webRootPath, vM.Product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldfilepath)) // this is to check if the file exists or not 
                        {
                            System.IO.File.Delete(oldfilepath);// this is to delete the file
                        }
                    }


                    using (var filestream = new FileStream(Path.Combine(ProductPath, filename), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }

                    vM.Product.ImageUrl = @"\images\product\" + filename;
                }

                    if(vM.Product.Id is 0)
                    {
                        // to create a new product
                        _unitOfWork.Product.Add(vM.Product);
                    }
                    else
                    {
                        // to update an existing product
                        _unitOfWork.Product.Update(vM.Product);
                    }
                

                
                _unitOfWork.Save();
                TempData["Success"] = "Product created successfully";
                return RedirectToAction(nameof(Index));// or return View("Index"); or rediect("Index");

            }
            else 
            // even if the model state is not valid the categorylist must be populated with the data that the user enteredz 
            {// if the model state is not valid then we need to return the view with the data that the user entered 
                vM.CategoryList = _unitOfWork.Category.GetAll().Select(
                    u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    });
                return View(vM);
            }
            
        }
        /*[HttpGet]
        public IActionResult Edit(int? id)
        {
            if(id is null or 0)
            {
                return NotFound();
            }
            var productfromdb = _unitOfWork.Product.Get(p => p.Id == id);

            if(productfromdb is null)
            {
                return NotFound();
            }
            return View(productfromdb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(product);
                _unitOfWork.Save();
                TempData["Success"] = "Product Updated successfully";
                return RedirectToAction(nameof(Index));// or return View("Index"); or rediect("Index");

            }
            return View(product);

        }*/
        /*[HttpGet]
        public IActionResult Delete(int? id)
        {
            if(id is null or 0)
            {
                return NotFound();
            }
            var productfromdb = _unitOfWork.Product.Get(p => p.Id == id);

            if(productfromdb is null)
            {
                return NotFound();
            }
            return View(productfromdb);
        }

        [HttpPost,ActionName("Delete")]
        public IActionResult DeleteProd(int? id)
        {
            var product = _unitOfWork.Product.Get(p => p.Id == id);
            if(product is null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(product);
            _unitOfWork.Save();
            TempData["Success"] = "Product Deleted successfully";
            return RedirectToAction(nameof(Index));

        }*/
        #region API CALLS
        [HttpGet]

        public IActionResult GetAll()
        {
            List<Product> products = _unitOfWork.Product.GetAll(includeProp: "Category").ToList();

            return Json(new { data = products });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var ProductToBeDeleted = _unitOfWork.Product.Get(u => u.Id == id);

            if(ProductToBeDeleted is null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var oldimage = Path.Combine(_hostEnvironment.WebRootPath, ProductToBeDeleted.ImageUrl.TrimStart('\\'));
            if(System.IO.File.Exists(oldimage))
            {
                System.IO.File.Delete(oldimage);
            }
            _unitOfWork.Product.Remove(ProductToBeDeleted);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });

        }
        #endregion
    }
}
