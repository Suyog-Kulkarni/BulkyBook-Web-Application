using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModel;
using BulkyBook.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public OrderVM orderVM { get; set; }

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        //[Authorize(Roles = SD.Role_Admin)]
        public IActionResult Details(int OrderId)
        {
            orderVM = new()
            {
                orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == OrderId, includeProp: "ApplicationUser"),
                orderDetails = _unitOfWork.OrderDetail.GetAll(u => u.OrderHeaderId == OrderId, includeProp: "Product")
            };
            return View(orderVM);
        }
        [HttpPost]
        [Authorize(Roles = SD.Role_Admin+","+SD.Role_Employee)]
        public IActionResult UpdateOrderDetail()
        {
            var orderHeaderFromDb = _unitOfWork.OrderHeader.Get(u =>u.Id == orderVM.orderHeader.Id);
            orderHeaderFromDb.Name = orderVM.orderHeader.Name;
            orderHeaderFromDb.PhoneNumber = orderVM.orderHeader.PhoneNumber;
            orderHeaderFromDb.StreetAddress = orderVM.orderHeader.StreetAddress;
            orderHeaderFromDb.City = orderVM.orderHeader.City;
            orderHeaderFromDb.State = orderVM.orderHeader.State;
            orderHeaderFromDb.PostalCode = orderVM.orderHeader.PostalCode;
            // ordderheaderfromdb is the object that is in the database which is being updated and the orderVM.orderHeader is the object that is being passed in from the view 
            // ordderheaderfromdb already contains the data the new input data will be updated to the database 
            // initially the orderVM.orderHeader is null and the orderHeaderFromDb is the object that is in the database 
            // the orderVM.orderHeader is being updated with the new data from the view and then the orderHeaderFromDb is being updated with the new data from the orderVM.orderHeader
            // the orderHeaderFromDb is then being saved to the database
            
            if (!String.IsNullOrEmpty(orderVM.orderHeader.Carrier))
            {
                orderHeaderFromDb.Carrier = orderVM.orderHeader.Carrier;
            }
            if (!String.IsNullOrEmpty(orderVM.orderHeader.TrackingNumber))
            {
                orderHeaderFromDb.TrackingNumber = orderVM.orderHeader.TrackingNumber;
            }

            _unitOfWork.OrderHeader.Update(orderHeaderFromDb);
            _unitOfWork.Save();
            TempData["Success"] = "Order Details Updated Successfully";
            
            return RedirectToAction(nameof(Details), new {OrderId = orderHeaderFromDb.Id });// this is the order id that is being passed in to the details action method 
            // the order id is being passed in to the details action method so that the details view can be updated with the new data
           
        }
        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult StartProcessing()
        {
            // the orderVM.orderHeader.Id is the order id that is being passed in from the view 
            // updatestatus is a method in the orderheaderrepository that is being used to update the order status to in process
            // the order status is being updated to in process because the order is being processed
            // it takes two parameters the first parameter is the order id and the second parameter is the order status
            // the OrderId is the id that is passed to the details action method and then passed to the startprocessing action method
            // because OrderId helps to identify the order that is being processed 
            _unitOfWork.OrderHeader.UpdateStatus(orderVM.orderHeader.Id, SD.StatusInProcess);
            _unitOfWork.Save();
            TempData["Success"] = "Order Details Updated Successfully";
            return RedirectToAction(nameof(Details), new {OrderId = orderVM.orderHeader.Id});
        }
        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult ShipOrder()
        {
            var orderHeaderFromDb = _unitOfWork.OrderHeader.Get(u => u.Id == orderVM.orderHeader.Id);

            orderHeaderFromDb.TrackingNumber = orderVM.orderHeader.TrackingNumber;
            orderHeaderFromDb.Carrier = orderVM.orderHeader.Carrier;
            orderHeaderFromDb.ShippingDate = DateTime.Now;
            orderHeaderFromDb.OrderStatus = SD.StatusShipped;

            if(orderHeaderFromDb.PaymentStatus == SD.PaymentStatusDelayedPayment)
            {
                orderHeaderFromDb.PaymentDueDate = DateTime.Now.AddDays(30);
            }// if the payment status is delayed payment then the payment due date is set to 30 days from the current date
            _unitOfWork.OrderHeader.Update(orderHeaderFromDb);
            _unitOfWork.Save();
            _unitOfWork.OrderHeader.UpdateStatus(orderVM.orderHeader.Id, SD.StatusShipped);
            _unitOfWork.Save();
            TempData["Success"] = "Order shipped Successfully";
            return RedirectToAction(nameof(Details), new { OrderId = orderVM.orderHeader.Id });
        }
        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult CancelOrder()
        { 
            var orderHeaderFromDb = _unitOfWork.OrderHeader.Get(u => u.Id == orderVM.orderHeader.Id);

            if (orderHeaderFromDb.PaymentStatus == SD.PaymentStatusApproved)
            {
                var options = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderHeaderFromDb.PaymentIntentId

                };
                var service = new RefundService();
                Refund refund = service.Create(options);

                _unitOfWork.OrderHeader.UpdateStatus(orderHeaderFromDb.Id, SD.StatusCancelled, SD.StatusRefunded);
            }
            else
            {
                _unitOfWork.OrderHeader.UpdateStatus(orderHeaderFromDb.Id, SD.StatusCancelled, SD.StatusCancelled);
            }
            _unitOfWork.Save();
            TempData["Success"] = "Order Cancelled Successfully";

            return RedirectToAction(nameof(Details), new { OrderId = orderVM.orderHeader.Id });
        }
        [HttpPost]
        public IActionResult Details_PAY_NOW()
        {
            orderVM.orderHeader = _unitOfWork.OrderHeader
                .Get(u => u.Id == orderVM.orderHeader.Id, includeProp: "ApplicationUser");
            // we are getting the order header from the database and then we are including the application user
            // the application user is the user that is logged in
            // the order header is the order that is being processed
            orderVM.orderDetails = _unitOfWork.OrderDetail
                .GetAll(u => u.OrderHeaderId == orderVM.orderHeader.Id, includeProp: "Product");
            // the order details are the details of the order that is being processed
            // the order details are being retrieved from the database and then the product is being included
            // the product is the product that is being ordered

            var domain = "https://localhost:7244/";
            // how to add google pay and apple pay to stripe payment page please tell me 
            // https://stripe.com/docs/payments/accept-a-payment?integration=elements 
            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"admin/order/PaymentConfirmation?orderHeaderId={orderVM.orderHeader.Id}",
                CancelUrl = domain + $"admin/order/details?orderId={orderVM.orderHeader.Id}",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",



            };

            foreach (var item in orderVM.orderDetails)
            {
                var sessionLineItem = new SessionLineItemOptions// this is the product that will be displayed on the stripe payment page 
                {
                    PriceData = new SessionLineItemPriceDataOptions// this is the price of the product that will be displayed on the stripe payment page 
                    {
                        UnitAmount = (long)(item.Price * 100),
                        Currency = "inr",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Title,// this is the name of the product that will be displayed on the stripe payment page 

                        }

                    },
                    Quantity = item.ProductCount
                };
                options.LineItems.Add(sessionLineItem);
            }
            var service = new SessionService();
            Session session = service.Create(options);
            _unitOfWork.OrderHeader.UpdateStripePaymentId(orderVM.orderHeader.Id, session.Id, session.PaymentIntentId);
            _unitOfWork.Save();

            Response.Headers.Add("Location", session.Url);//  
            return new StatusCodeResult(303);
        }
        public IActionResult PaymentConfirmation(int orderHeaderId)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == orderHeaderId, includeProp: "ApplicationUser");
            if (orderHeader.PaymentStatus is SD.PaymentStatusDelayedPayment)
            {
                // this is an order that is paid by company
                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);

                if (session.PaymentStatus.ToLower() is "paid")
                {
                    _unitOfWork.OrderHeader.UpdateStripePaymentId(orderHeaderId, session.Id, session.PaymentIntentId);
                    _unitOfWork.OrderHeader.UpdateStatus(orderHeaderId, orderHeader.OrderStatus, SD.PaymentStatusApproved);
                    _unitOfWork.Save();
                }
            }

            return View(orderHeaderId);
        }

        #region API CALLS
        [HttpGet]

        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeader> objOrderHeaders;

            if(User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Employee))
            {
                objOrderHeaders = _unitOfWork.OrderHeader.GetAll(includeProp: "ApplicationUser").ToList();
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var UserId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                objOrderHeaders = _unitOfWork.OrderHeader.GetAll(u => u.ApplicationUserId == UserId, includeProp: "ApplicationUser").ToList();
            }
            switch (status)
            {
                case "inprocess":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus==SD.StatusInProcess);
                    break;
                case "pending":
                    objOrderHeaders = objOrderHeaders.Where(u => u.PaymentStatus == SD.StatusPending);
                    break;
                case "completed":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == SD.StatusShipped);
                    break;
                case "approved":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == SD.StatusApproved);
                    break;
                default:
                    break;
            }
            return Json(new { data = objOrderHeaders });
        }
        #endregion 
    }
}
