using JuanShoesStore.Models;
using JuanShoesStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JuanShoesStore.Controllers
{
    public class OrderController : Controller
    {
        private readonly JuanContext _juanContext;
        private readonly UserManager<AppUser> _userManager;

        public OrderController(JuanContext juanContext, UserManager<AppUser> userManager)
        {
            _juanContext = juanContext;
            _userManager = userManager;
        }

        #region Checkout

        public IActionResult Checkout()
        {
            AppUser user = null;
            if (User.Identity.IsAuthenticated)
            {
                user = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            }


            CheckoutViewModel checkoutVM = new CheckoutViewModel
            {
                Order = new Order
                {
                    Email = user?.Email,
                    Address = user?.Address,
                    City = user?.City,
                    Country = user?.Country,
                    PhoneNumber = user?.PhoneNumber,
                    FullName = user?.FullName,
                },
                BasketVM = _getBasket(user),

            };

            return View(checkoutVM);
        }

        #endregion

        #region getBasket

        private BasketViewModel _getBasket(AppUser user)
        {
            BasketViewModel basketVM = new BasketViewModel
            {
                BasketTotalPrice = 0,
                BasketItemDVM = new List<BasketItemDetaildViewModel>(),
            };
            List<BasketItemViewModel> basketItemsVM = new List<BasketItemViewModel>();
            if (user == null)
            {
                string basketItemsStr = HttpContext.Request.Cookies["basket"];
                if (!string.IsNullOrWhiteSpace(basketItemsStr))
                    basketItemsVM = JsonConvert.DeserializeObject<List<BasketItemViewModel>>(basketItemsStr);
            }
            else
            {
                basketItemsVM = _juanContext.BasketItems
                    .Where(x => x.AppUserId == user.Id)
                    .Select(x => new BasketItemViewModel { Count = x.Count, ShoeId = x.ShoeId })
                    .ToList();
            }

            foreach (BasketItemViewModel item in basketItemsVM)
            {
                Shoe shoe = _juanContext.Shoes.FirstOrDefault(x => x.Id == item.ShoeId);

                BasketItemDetaildViewModel basketItemDVM = new BasketItemDetaildViewModel
                {
                    Shoe = shoe,
                    Count = item.Count
                };

                basketVM.BasketItemDVM.Add(basketItemDVM);
                decimal price = shoe.DiscountPercent > 0 ? shoe.SalePrice * (1 - ((decimal)shoe.DiscountPercent / 100)) : shoe.SalePrice;
                basketVM.BasketTotalPrice += price * item.Count;
            }

            return basketVM;
        }

        #endregion

        #region Create

        [HttpPost]
        public IActionResult Create(Order order)
        {
            AppUser user = null;
            if (User.Identity.IsAuthenticated)
                user = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name && !x.IsAdmin);

            CheckoutViewModel checkoutVM = new CheckoutViewModel
            {
                BasketVM = _getBasket(user),
                Order = order,
            };

            if (!ModelState.IsValid)
                return View("Checkout", checkoutVM);

            if (checkoutVM.BasketVM.BasketItemDVM.Count == 0)
            {
                ModelState.AddModelError("", "First you have to choose product!");
                return View("Checkout", checkoutVM);
            }

            order.AppUserId = user?.Id;
            order.CreatedAt = DateTime.UtcNow.AddHours(4);
            order.ModifiedAt = DateTime.UtcNow.AddHours(4);
            order.OrderStatus = Enums.OrderStatus.Pending;

            order.OrderItems = new List<OrderItem>();

            foreach (var item in checkoutVM.BasketVM.BasketItemDVM)
            {
                OrderItem orderItem = new OrderItem
                {
                    CostPrice = item.Shoe.CostPrice,
                    SalePrice = item.Shoe.SalePrice,
                    ShoeId = item.Shoe.Id,
                    DiscountedPrice = item.Shoe.DiscountPercent > 0 ? item.Shoe.SalePrice * (1 - (decimal)item.Shoe.DiscountPercent / 100) : item.Shoe.SalePrice,
                    Count = item.Count,

                };
                order.OrderItems.Add(orderItem);
                order.TotalPrice += orderItem.DiscountedPrice * item.Count;

            }
            _juanContext.Orders.Add(order);

            if (user == null)
                HttpContext.Response.Cookies.Delete("basket");
            else
                _juanContext.BasketItems.RemoveRange(_juanContext.BasketItems.Where(x => x.AppUserId == user.Id));

            _juanContext.SaveChanges();

            return RedirectToAction("profile", "account");
        }
        #endregion
        

    }
}
