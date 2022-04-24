using JuanShoesStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace JuanShoesStore.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles ="SuperAdmin, Admin")]
    public class BrandController : Controller
    {
        private readonly JuanContext _context;
        public BrandController(JuanContext context)
        {
            _context = context;
        }

        public IActionResult Index(bool? isDeleted ,int page = 1)
        {
            if (page < 1)
            {
                return NotFound();
            }

            ViewBag.IsDeleted = isDeleted;
            var brands = _context.Brands.Include(x => x.Shoes).AsQueryable();

            if (isDeleted != null)
            {
                brands = brands.Where(x => x.IsDeleted == isDeleted);
            }

            string itemPerPageStr = _context.Setting.FirstOrDefault(x => x.Key == "ItemPerPage").Value;
            int itemPerPage = string.IsNullOrEmpty(itemPerPageStr) ? 3 : int.Parse(itemPerPageStr);
            return View(Pagination<Brand>.Create(brands, page, itemPerPage));
        }



        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Brand brand)
        {
            if (!ModelState.IsValid)
                return View();


            if (_context.Brands.Any(x => x.Name == brand.Name))
            {
                ModelState.AddModelError("Name", "This Brand Already Exists");
                return View();
            }
            brand.CreatedAt = DateTime.UtcNow.AddHours(4);

            _context.Brands.Add(brand);
            _context.SaveChanges();

            return RedirectToAction("index");
        }


        public IActionResult Delete(int id)
        {

            Brand brand = _context.Brands.FirstOrDefault(x => x.Id == id);
            if (brand == null)
            {
                return BadRequest();
            }
            _context.Remove(brand);

            brand.IsDeleted = true;
            brand.DeletedAt = DateTime.UtcNow.AddHours(4);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            Brand brand = _context.Brands.FirstOrDefault(x => x.Id == id);
            if (brand == null)
            {
                return BadRequest();
            }
            return View(brand);
        }

        [HttpPost]
        public IActionResult Edit(Brand brand)
        {
            Brand tempBrand = _context.Brands.FirstOrDefault(x => x.Id == brand.Id);
            if (tempBrand.Name == brand.Name)
            {
                ModelState.AddModelError("Name", "This Brand Already Exists");
                return View();
            }

            tempBrand.ModifiedAt = DateTime.UtcNow.AddHours(4);

            tempBrand.Name = brand.Name;
            _context.SaveChanges();

            return RedirectToAction("index");
        }

    }
}
