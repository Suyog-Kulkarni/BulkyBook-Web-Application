using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkyBookWeb.ViewComponents;

public class ShoppingCartViewComponent : ViewComponent
{
    private readonly IUnitOfWork _unitOfWork;
    public ShoppingCartViewComponent(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IViewComponentResult Invoke()
    {
        var cliamsIdentity = (ClaimsIdentity)User.Identity;
        var claims = cliamsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        if (claims is not null)
        {
            if (HttpContext.Session.GetInt32(SD.SessionCart) is null)
            {
                HttpContext.Session.SetInt32(SD.SessionCart,
                         _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claims.Value).Count());
            }
            return View(HttpContext.Session.GetInt32(SD.SessionCart));
        }
        // above code shows the count of shopping cart after the logouts and logins afterwards

        else
        {
            HttpContext.Session.Clear();
            return View(0);
        }
    }
}

