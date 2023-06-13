using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> products = _unitOfWork.Product.GetAll().ToList();
            return View(products);
        }
        // GET 
        public IActionResult Create()
        {
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(product);
                _unitOfWork.Save();
                TempData["Success"] = "Product created successfully";
                return RedirectToAction(nameof(Index));// or return View("Index"); or rediect("Index");

            }
            return View("Index","Category");
            
        }
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
            _unitOfWork.Product.Update(product);
            _unitOfWork.Save();
            TempData["Success"] = "Product Updated successfully";
            return RedirectToAction(nameof(Index));// or return View("Index"); or rediect("Index");


        }
        [HttpGet]
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

        }
    }
}
