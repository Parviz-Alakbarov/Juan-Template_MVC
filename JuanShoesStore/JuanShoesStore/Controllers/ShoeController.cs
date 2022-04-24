using JuanShoesStore.Models;
using JuanShoesStore.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JuanShoesStore.Controllers
{
    public class ShoeController : Controller
    {
        private readonly JuanContext _context;
        private readonly UserManager<AppUser> _userManager;
        public ShoeController(JuanContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        #region Index
        public IActionResult Index(bool? gender, List<int> brandIds, List<int> colorIds, decimal? minPrice, decimal? maxPrice, int page = 1)
        {
            if (page < 1)
            {
                return NotFound();
            }
            string itemPerPageStr = _context.Setting.FirstOrDefault(x => x.Key == "ItemPerPage").Value;
            int itemPerPage = string.IsNullOrEmpty(itemPerPageStr) ? 3 : int.Parse(itemPerPageStr);

            var shoes = _context.Shoes.Include(x => x.Brand).Include(x => x.ShoeColorItems).ThenInclude(x => x.Color).Where(x => !x.IsDeleted).AsQueryable();

            ViewBag.Gender = gender;
            ViewBag.PageIndex = page;
            if (gender!=null)
                shoes= shoes.Where(x => x.Gender == gender);

            if (brandIds != null && brandIds.Count > 0)
                shoes = shoes.Where(x => brandIds.Any(br => br == x.BrandId));


            if (colorIds != null && colorIds.Count > 0)
                shoes = shoes.Where(x => x.ShoeColorItems.Any(cl => colorIds.Contains(cl.ColorId)));


            ShopViewModel shopVM = new ShopViewModel();

            if (shoes.Any())
            {
                shopVM.MinPrice = shoes.Min(x => x.SalePrice);
                shopVM.MaxPrice = shoes.Max(x => x.SalePrice);
            }

            ViewBag.FilterMinPrice = minPrice ?? shopVM.MinPrice;
            ViewBag.FilterMaxPrice = maxPrice ?? shopVM.MaxPrice;

            if (minPrice != null && maxPrice != null)
                shoes = shoes.Where(x => x.SalePrice >= minPrice && x.SalePrice <= maxPrice);

            shopVM.Shoes = Pagination<Shoe>.Create(shoes, page, itemPerPage);
            shopVM.Brands = _context.Brands.Include(x => x.Shoes).ToList();
            shopVM.Settings = _context.Setting.ToDictionary(x => x.Key, x => x.Value);
            shopVM.Colors = _context.Colors.Include(x => x.ShoeColorItems).ThenInclude(x => x.Shoe).ToList();
            shopVM.FilterColorIds = colorIds;
            shopVM.FilterBrandIds = brandIds;
            shopVM.MaleGenderCount = _context.Shoes.Where(x => x.Gender == true).Count();
            shopVM.FemaleGenderCount = _context.Shoes.Where(x => x.Gender == false).Count();


            return View(shopVM);
        }
        #endregion

        #region Detail

        public IActionResult Detail(int id)
        {
            /*.Include(x => x.ShoeComments.OrderByDescending(x => x.CreatedAt).Take(4))*/
            Shoe shoe = _context.Shoes
                .Include(x => x.Brand)
                .Include(x => x.Category)
                .Include(x => x.ShoeComments).ThenInclude(x => x.AppUser)
                .Include(x => x.ShoeColorItems).ThenInclude(x => x.Color)
                .FirstOrDefault(x => x.Id == id);
            if (shoe == null) { return RedirectToAction("error", "home"); }

            ShoeDetailViewModel shoeDVM = new ShoeDetailViewModel
            {
                Shoe = shoe,
                Comment = new ShoeComment { ShoeId = shoe.Id },
                RelatedShoes = _context.Shoes.Where(x => x.CategoryId == shoe.CategoryId).Take(5).ToList(),
            };

            return View(shoeDVM);
        }
        #endregion

        #region GetShoe

        public IActionResult GetShoe(int id)
        {
            Shoe shoe = _context.Shoes.Include(x => x.Brand).Include(x => x.Category).Include(x => x.ShoeColorItems).ThenInclude(x => x.Color).FirstOrDefault(x => x.Id == id);
            if (shoe == null) { return View("error", "home"); }

            return PartialView("_ShoeQuickViewModel", shoe);
        }
        #endregion

        #region AddToBasket

        public IActionResult AddToBasket(int id)
        {
            if (!_context.Shoes.Any(x => x.Id == id && !x.IsDeleted))
                return RedirectToAction("error", "home");

            AppUser user = null;
            if (User.Identity.IsAuthenticated)
            {
                user = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name && !x.IsAdmin);
            }

            if (user == null)
            {
                string basketItemsStr = HttpContext.Request.Cookies["basket"];
                List<BasketItemViewModel> basketItemList = new List<BasketItemViewModel>();
                if (!string.IsNullOrWhiteSpace(basketItemsStr))
                    basketItemList = JsonConvert.DeserializeObject<List<BasketItemViewModel>>(basketItemsStr);

                BasketItemViewModel item = basketItemList.FirstOrDefault(x => x.ShoeId == id);

                if (item == null)
                {
                    item = new BasketItemViewModel
                    {
                        ShoeId = id,
                        Count = 1,
                    };
                    basketItemList.Add(item);
                }
                else
                    item.Count++;

                basketItemsStr = JsonConvert.SerializeObject(basketItemList);

                HttpContext.Response.Cookies.Append("basket", basketItemsStr);

                TempData["Success"] = "Product Added successfully";

                return PartialView("_BasketPartial", _getBasket(basketItemList));
            }
            else
            {
                BasketItem basketItem = _context.BasketItems.FirstOrDefault(x => x.ShoeId == id && x.AppUserId == user.Id);
                if (basketItem == null)
                {
                    basketItem = new BasketItem
                    {
                        ShoeId = id,
                        AppUserId = user.Id,
                        Count = 1,
                        CreatedAt = DateTime.UtcNow.AddHours(4),

                    };
                    _context.BasketItems.Add(basketItem);
                }
                else
                    basketItem.Count++;

                _context.SaveChanges();

                TempData["Success"] = "Product Added successfully";

                return PartialView("_BasketPartial", _getBasket(_context.BasketItems.Where(x => x.AppUserId == user.Id).ToList()));
            }
        }
        #endregion

        #region RemoveFromBasket

        public IActionResult RemoveFromBasket(int id)
        {
            AppUser user = null;
            if (User.Identity.IsAuthenticated)
            {
                user = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name && !x.IsAdmin);
            }

            if (user == null)
            {
                string basketItemsStr = HttpContext.Request.Cookies["basket"];
                List<BasketItemViewModel> basketItemList = new List<BasketItemViewModel>();
                if (!string.IsNullOrWhiteSpace(basketItemsStr))
                    basketItemList = JsonConvert.DeserializeObject<List<BasketItemViewModel>>(basketItemsStr);

                BasketItemViewModel item = basketItemList.FirstOrDefault(x => x.ShoeId == id);

                if (item == null)
                    return BadRequest();
                else
                    basketItemList.Remove(item);

                basketItemsStr = JsonConvert.SerializeObject(basketItemList);

                HttpContext.Response.Cookies.Append("basket", basketItemsStr);

                TempData["Warning"] = "Product Removed successfully";

                return PartialView("_BasketPartial", _getBasket(basketItemList));
            }
            else
            {
                BasketItem basketItem = _context.BasketItems.FirstOrDefault(x => x.ShoeId == id && x.AppUserId == user.Id);
                if (basketItem == null)
                    return BadRequest();
                else
                    _context.BasketItems.Remove(basketItem);

                _context.SaveChanges();

                TempData["Warning"] = "Product Removed successfully";

                return PartialView("_BasketPartial", _getBasket(_context.BasketItems.Where(x => x.AppUserId == user.Id).ToList()));
            }
        }

        #endregion

        #region Comment

        [HttpPost]
        public IActionResult Comment(ShoeComment comment)
        {
            AppUser user = null;
            if (User.Identity.IsAuthenticated)
                user = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name && !x.IsAdmin);


            if (user == null)
                return RedirectToAction("login", "account");

            Shoe shoe = _context.Shoes
                 .Include(x => x.Brand)
                 .Include(x => x.Category)
                 .Include(x => x.ShoeComments)
                 .Include(x => x.ShoeColorItems).ThenInclude(x => x.Color)
                 .FirstOrDefault(x => x.Id == comment.ShoeId && !x.IsDeleted);

            if (shoe == null)
                return RedirectToAction("error", "home");

            ShoeDetailViewModel shoeDVM = new ShoeDetailViewModel
            {
                Comment = comment,
                Shoe = shoe,
                RelatedShoes = _context.Shoes.Where(x => x.CategoryId == shoe.CategoryId).Take(5).ToList(),
            };

            if (_context.ShoeComments.Where(x => x.AppUserId == user.Id).ToList().Count > 1)
            {
                ModelState.AddModelError("", "You already commented on this shoe");
                return View("detail", shoeDVM);
            };

            if (!ModelState.IsValid)
            {
                return View("detail", shoeDVM);
            }

            comment.AppUserId = user.Id;
            comment.CreatedAt = DateTime.UtcNow.AddHours(4);
            shoe.ShoeComments.Add(comment);
            shoe.Rate = (int)Math.Ceiling(shoe.ShoeComments.Sum(x => x.Rate) / (double)shoe.ShoeComments.Count);

            _context.SaveChanges();
            TempData["Success"] = "Comment Posted Successfully!";
            return RedirectToAction("detail", new { id = shoe.Id });

        }
        #endregion

        #region GetBasket

        public BasketViewModel _getBasket(List<BasketItem> basketItems)
        {
            BasketViewModel basketVM = new BasketViewModel
            {
                BasketTotalPrice = 0,
                BasketItemDVM = new List<BasketItemDetaildViewModel>(),
                TaxPercent = Decimal.Parse(_context.Setting.FirstOrDefault(x => x.Key == "TaxPercent").Value),
                VatPercent = Decimal.Parse(_context.Setting.FirstOrDefault(x => x.Key == "VatPercent").Value),
            };

            foreach (BasketItem item in basketItems)
            {
                Shoe shoe = _context.Shoes.Include(x => x.Brand).FirstOrDefault(x => x.Id == item.ShoeId);

                /*if (shoe == null) { return BadRequest(); }*/

                BasketItemDetaildViewModel basketItemDVM = new BasketItemDetaildViewModel
                {
                    Shoe = shoe,
                    Count = item.Count
                };
                basketVM.BasketItemDVM.Add(basketItemDVM);
                decimal price = shoe.DiscountPercent > 0 ? (shoe.SalePrice * (1 - (decimal)(shoe.DiscountPercent) / 100)) : shoe.SalePrice;

                basketVM.BasketTotalPrice += price * item.Count;
            }

            return basketVM;
        }

        public BasketViewModel _getBasket(List<BasketItemViewModel> basketItems)
        {
            BasketViewModel basketVM = new BasketViewModel
            {
                BasketTotalPrice = 0,
                BasketItemDVM = new List<BasketItemDetaildViewModel>(),
            };

            foreach (BasketItemViewModel item in basketItems)
            {
                Shoe shoe = _context.Shoes.Include(x => x.Brand).FirstOrDefault(x => x.Id == item.ShoeId);

                /*if (shoe == null) { return BadRequest(); }*/

                BasketItemDetaildViewModel basketItemDVM = new BasketItemDetaildViewModel
                {
                    Shoe = shoe,
                    Count = item.Count
                };
                basketVM.BasketItemDVM.Add(basketItemDVM);
                decimal price = shoe.DiscountPercent > 0 ? (shoe.SalePrice * (1 - (decimal)shoe.DiscountPercent / 100)) : shoe.SalePrice;

                basketVM.BasketTotalPrice += price * item.Count;
            }

            return basketVM;
        }
        #endregion

    }
}
