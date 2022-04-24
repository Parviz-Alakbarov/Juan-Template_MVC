using JuanShoesStore.Models;
using JuanShoesStore.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace JuanShoesStore.Services
{
    public class LayoutService
    {
        private readonly JuanContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;
        public LayoutService(JuanContext context, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }


        public Dictionary<string, string> GetSettings()
        {
            return _context.Setting.ToDictionary(x => x.Key, x => x.Value);
        }

        public BasketViewModel GetBasket()
        {
            BasketViewModel basketVM = new BasketViewModel
            {
                BasketTotalPrice = 0,
                BasketItemDVM = new List<BasketItemDetaildViewModel>(),
            };

            AppUser user = null;
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                user = _userManager.Users.FirstOrDefault(x => x.UserName == _httpContextAccessor.HttpContext.User.Identity.Name);
            }

            List<BasketItemViewModel> basketItemVM = new List<BasketItemViewModel>();

            if (user == null)
            {
                string basketItemsStr = _httpContextAccessor.HttpContext.Request.Cookies["basket"];
                if (!string.IsNullOrWhiteSpace(basketItemsStr))
                    basketItemVM = JsonConvert.DeserializeObject<List<BasketItemViewModel>>(basketItemsStr);
            }
            else
            {
                basketItemVM = _context.BasketItems.Where(x => x.AppUserId == user.Id).Select(x => new BasketItemViewModel { ShoeId = x.ShoeId, Count = x.Count }).ToList();
            }

            foreach (BasketItemViewModel item in basketItemVM)
            {
                Shoe shoe = _context.Shoes.Include(x => x.Brand).FirstOrDefault(x => x.Id == item.ShoeId);
                BasketItemDetaildViewModel basketItemDVM = new BasketItemDetaildViewModel
                {
                    Count = item.Count,
                    Shoe = shoe,
                };

                basketVM.BasketItemDVM.Add(basketItemDVM);
                decimal price = shoe.DiscountPercent > 0 ? (shoe.SalePrice * (1 - (decimal)shoe.DiscountPercent / 100)) : shoe.SalePrice;
                basketVM.BasketTotalPrice += price * item.Count;

            }
            return basketVM;
        }
    }
}
