﻿using Bulky.Models;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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
            IEnumerable<Product> products= _unitOfWork.Product.GetAll(includeProp:"Category");
            return View(products);
        }

        public IActionResult Details(int id)
        {
            Product product = _unitOfWork.Product.Get(u=>u.Id== id, includeProp: "Category");
            return View(product);
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