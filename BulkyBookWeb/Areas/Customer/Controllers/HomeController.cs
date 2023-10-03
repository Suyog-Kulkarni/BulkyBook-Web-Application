using Bulky.Models;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModel;
using BulkyBook.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BulkyBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        /*[Route("")]
        [Route("Home")]
        [Route("Home/Index")]*/ /*if url have any of these then index action will run*/
        /* error occurs if default routing in not on so use above annotations to use cousyomly*/

        public IActionResult Index()
        {
            var cliamsIdentity = (ClaimsIdentity)User.Identity;
            var claims = cliamsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if(claims is not null)
            {
                HttpContext.Session.SetInt32(SD.SessionCart, 
                             _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claims.Value).Count());
            }
            // above code shows the count of shopping cart after the logouts and logins afterwards 
            IEnumerable<Product> products= _unitOfWork.Product.GetAll(includeProp:"Category");
            return View(products);
        }

        public IActionResult Details(int id)
        {
            ShoppingCart cart = new()
            {
                Product = _unitOfWork.Product.Get(u => u.Id == id, includeProp: "Category"),
                Count = 1,
                ProductId = id
            };
            return View(cart);
        }
        [HttpPost]
        [Authorize]// authorize is impoartant to get that particualr user information
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            shoppingCart.Id = 0;
            var cliamsIdentity = (ClaimsIdentity)User.Identity;
            var userId = cliamsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = userId;// this is used to get the user id of the user who is logged in
            // we are getting the user id of the user who is logged in and storing it in applicationuserid
            

            ShoppingCart cartfromdb = _unitOfWork.ShoppingCart.Get(u => u.ApplicationUserId == userId &&
            u.ProductId == shoppingCart.ProductId);// check if cart already exists for that product for that user or not 

            if(cartfromdb is not null)
            {
                // cart already exists for that product for that user
                cartfromdb.Count += shoppingCart.Count;// add the count to the existing count
                _unitOfWork.ShoppingCart.Update(cartfromdb);
                _unitOfWork.Save();
            }
            else
            {
                // no cart exists for that product for that user
                _unitOfWork.ShoppingCart.Add(shoppingCart);
                _unitOfWork.Save();
                HttpContext.Session.SetInt32(SD.SessionCart, 
                    _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId).Count());
                // this is used to store the session in memory cache 
                // we re getting all the shopping cart of that user and storing it in session 
                // so that we can use it in other pages
                // we are storing the count of shopping cart in session

            }
            
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}