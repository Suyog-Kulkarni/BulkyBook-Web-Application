﻿using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartVM ShoppingCartVM { get; set; } = null!;

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            var UserId = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == UserId,
                includeProp: "Product")
            };

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuanity(cart);
                ShoppingCartVM.OrderTotal += (cart.Price * cart.Count);
            }
            return View(ShoppingCartVM);
        }

        public IActionResult Plus(int cartId)
        {
            var cartfromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            cartfromDb.Count += 1;
            _unitOfWork.ShoppingCart.Update(cartfromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Minus(int cartId)
        {
            var cartfromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            if (cartfromDb.Count <= 1)
            {
                //remove that cart
                _unitOfWork.ShoppingCart.Remove(cartfromDb);
            }
            else
            {
                cartfromDb.Count -= 1;
                _unitOfWork.ShoppingCart.Update(cartfromDb);

            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Remove(int cartId)
        {
            var cartfromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            _unitOfWork.ShoppingCart.Remove(cartfromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Summary()
        {
            return View();
        }

        private double GetPriceBasedOnQuanity(ShoppingCart shoppingcart)
        {
            // this function will return the price based on the quantity of the product in the cart 
            if (shoppingcart.Count <= 50)
            {
                return shoppingcart.Product.Price;
            }
            else if (shoppingcart.Count <= 100)
            {
                return shoppingcart.Product.Price50;
            }
            else
            {
                return shoppingcart.Product.Price100;
            }
        }
    }
}
