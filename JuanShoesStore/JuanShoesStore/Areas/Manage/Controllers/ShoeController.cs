using JuanShoesStore.Models;
using JuanShoesStore.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JuanShoesStore.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class ShoeController : Controller
    {
        private readonly JuanContext _context;
        private readonly IWebHostEnvironment _environment;

        public ShoeController(JuanContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IActionResult Index(bool? isDeleted=null,int page=1)
        {
            if (page < 1)
            {
                return NotFound();
            }

            ViewBag.IsDeleted = isDeleted; 
            var shoes = _context.Shoes.Include(x => x.Brand).Include(x=>x.Category).Include(x => x.ShoeColorItems).ThenInclude(x => x.Color).AsQueryable();

            if (isDeleted!=null)
            {
                shoes = shoes.Where(x => x.IsDeleted == isDeleted);
            }

            string itemPerPageStr = _context.Setting.FirstOrDefault(x => x.Key == "ItemPerPage").Value;
            int itemPerPage = string.IsNullOrEmpty(itemPerPageStr) ? 3 : int.Parse(itemPerPageStr);
            return View(Pagination<Shoe>.Create(shoes, page, itemPerPage));
         }

        public IActionResult Edit(int id)
        {
            Shoe shoe = _context.Shoes.Include(x=>x.Brand).Include(x=>x.Category).Include(x=>x.ShoeColorItems).ThenInclude(x=>x.Color).FirstOrDefault(x => x.Id == id);

            if (shoe == null) { return BadRequest(); }

            ViewBag.Colors = _context.Colors.ToList();
            ViewBag.Brands = _context.Brands.Where(x => !x.IsDeleted).ToList();
            ViewBag.Categories = _context.Categories.ToList();


            return View(shoe);
        }
        [HttpPost]
        public IActionResult Edit(Shoe shoe)
        {
            ViewBag.Colors = _context.Colors.ToList();
            ViewBag.Brands = _context.Brands.Where(x => !x.IsDeleted).ToList();
            ViewBag.Categories = _context.Categories.ToList();


            if (!ModelState.IsValid)
                return View();

            if (shoe.SalePrice < 0)
            {
                ModelState.AddModelError("SalePrice", "Sale Price can't be negative!!!");
                return View();
            }

            if (shoe.CostPrice < 0)
            {
                ModelState.AddModelError("CostPrice", "Cost Price can't be negative!!!");
                return View();
            }

            if (shoe.SalePrice < shoe.CostPrice)
            {
                ModelState.AddModelError("SalePrice", "Sale Price can't be less than Cost Price!!!");
                return View();
            }

            if (shoe.DiscountPercent < 0)
            {
                ModelState.AddModelError("DiscountPercent", "Discount Percent can't be negative!!!");
                return View();
            }

           
            if (_context.Brands.Any(x => x.Id == shoe.Id))
            {
                ViewBag.ErrorMessage = "Brand Not Found!";
                RedirectToAction("error", "home");
            }

            
            Shoe existShoe= _context.Shoes.Include(x => x.Brand).Include(x=>x.Category).Include(x => x.ShoeColorItems).ThenInclude(x => x.Color).FirstOrDefault(x=>x.Id == shoe.Id);

            if (existShoe == null) { ViewBag.ErrorMessage = $"Shoe with this id-{shoe.Id} Not Found"; return RedirectToAction("error", "home"); }

            if (shoe.ImageFile != null)
            {
                bool isDeleted = FileManager.Delete(Constants.UploadImagesFolderPath,"product", existShoe.Image);
                if (!isDeleted)
                    return BadRequest();
                existShoe.Image = FileManager.Save(Constants.UploadImagesFolderPath, "product", shoe.ImageFile);
            }
            
            existShoe.Gender = shoe.Gender;
            existShoe.Name = shoe.Name;
            existShoe.SalePrice = shoe.SalePrice;
            existShoe.CostPrice = shoe.CostPrice;
            existShoe.DiscountPercent = shoe.DiscountPercent;
            existShoe.IsNew = shoe.IsNew;
            existShoe.IsAvailable = shoe.IsAvailable;
            existShoe.Rate = shoe.Rate;
            existShoe.Desc = shoe.Desc;

            existShoe.ModifiedAt = DateTime.UtcNow.AddHours(4);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            Shoe existShoe = _context.Shoes.FirstOrDefault(x => x.Id == id);

            if (existShoe == null) { return NotFound(); }
           
            existShoe.IsDeleted = true;
            existShoe.DeletedAt = DateTime.UtcNow.AddHours(4);

            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Restore(int id)
        {
            Shoe existShoe = _context.Shoes.FirstOrDefault(x => x.Id == id);

            if (existShoe == null) { return NotFound(); }

            existShoe.IsDeleted = false;
            existShoe.ModifiedAt = DateTime.UtcNow.AddHours(4);

            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Create()
        {
            ViewBag.Colors = _context.Colors.ToList();
            ViewBag.Brands = _context.Brands.Where(x => !x.IsDeleted).ToList();
            ViewBag.Categories = _context.Categories.ToList();

            return View();
        }
        [HttpPost]
        public IActionResult Create(Shoe shoe)
        {
            ViewBag.Colors = _context.Colors.ToList();
            ViewBag.Brands = _context.Brands.Where(x => !x.IsDeleted).ToList();
            ViewBag.Categories = _context.Categories.ToList();

            if (!ModelState.IsValid)
                return View();

            if (shoe.SalePrice < 0)
            {
                ModelState.AddModelError("SalePrice", "Sale Price can't be negative!!!");
                return View();
            }

            if (shoe.CostPrice < 0)
            {
                ModelState.AddModelError("CostPrice", "Cost Price can't be negative!!!");
                return View();
            }

            if (shoe.SalePrice < shoe.CostPrice)
            {
                ModelState.AddModelError("SalePrice", "Sale Price can't be less than Cost Price!!!");
                return View();
            }

            if (shoe.DiscountPercent < 0)
            {
                ModelState.AddModelError("DiscountPercent", "Discount Percent can't be negative!!!");
                return View();
            }

            if (shoe.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Image file is required!!!");
                return View();
            }
            if (_context.Brands.Any(x=>x.Id == shoe.Id))
            {
                ViewBag.ErrorMessage = "Brand Not Found!";
                RedirectToAction("error", "home");
            }
            if (_context.Categories.Any(x => x.Id == shoe.CategoryId))
            {
                ViewBag.ErrorMessage = "Category Not Found!";
                RedirectToAction("error", "home");
            }

            shoe.ShoeColorItems = new List<ShoeColorItem>();

            if (shoe.ColorIds != null)
            {
                foreach (var colorId in shoe.ColorIds)
                {
                    if (_context.Colors.Any(x => x.Id == colorId))
                    {
                        ShoeColorItem shoeColor = new ShoeColorItem
                        {
                             ColorId= colorId,
                        };
                        shoe.ShoeColorItems.Add(shoeColor);
                    }
                    else
                    {
                        ModelState.AddModelError("colorIds", "Color not found");
                        return View();
                    }
                }
            }

            Category category = _context.Categories.FirstOrDefault(x => x.Id == shoe.CategoryId);
            shoe.Category = category;
            shoe.Image = FileManager.Save(Constants.UploadImagesFolderPath, "product", shoe.ImageFile);
            shoe.CreatedAt = DateTime.UtcNow.AddHours(4);
            _context.Shoes.Add(shoe);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

    }
}
