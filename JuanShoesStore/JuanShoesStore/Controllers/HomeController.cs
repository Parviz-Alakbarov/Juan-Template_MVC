using JuanShoesStore.Models;
using JuanShoesStore.Util;
using JuanShoesStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JuanShoesStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly JuanContext _context;
        public HomeController(JuanContext context)
        {
            _context = context;
        }

        #region Index

        public IActionResult Index()
        {
            HomeViewModel homeViewModel = new HomeViewModel
            {
                Sliders = _context.Sliders.ToList(),
                Settings = _context.Setting.ToDictionary(x => x.Key, x => x.Value),
                BestSellerShoes = _context.Shoes.Take(6).ToList(),
                NewShoes = _context.Shoes.Where(x => x.IsNew == true).Take(6).ToList(),
                Partners = _context.Partners.ToList(),
            };
            return View(homeViewModel);
        }
        #endregion


        #region Error

        [Route("error")]
        public IActionResult Error()
        {
            return View();
        }
        #endregion

        #region ContactUs

        public IActionResult ContactUs()
        {
            ContactUsViewModel contactUsViewModel = new ContactUsViewModel
            {
                Settings = _context.Setting.ToDictionary(x => x.Key, x => x.Value),
            };

            return View(contactUsViewModel);
        }
        [HttpPost]
        public IActionResult ContactUs(ContactUs contactUs)
        {
            ContactUsViewModel viewModel = new ContactUsViewModel
            {
                ContactUs = contactUs,
                Settings = _context.Setting.ToDictionary(x => x.Key, x => x.Value),
            };
            if (!ModelState.IsValid)
            {
                return View("ContactUs", viewModel);
            }

            _context.ContactUses.Add(contactUs);
            _context.SaveChanges();

            TempData["Success"] = "Your Message Sent Successfully";
            return Ok();
        }



        #endregion
    }
}
