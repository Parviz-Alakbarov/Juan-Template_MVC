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
    public class ColorController :Controller
    {
        private readonly JuanContext _context;

        public ColorController(JuanContext context)
        {
            _context = context;
        }

        public IActionResult Index(int page = 1)
        {
            if (page < 1)
            {
                return NotFound();
            }

            string itemPerPageStr = _context.Setting.FirstOrDefault(x => x.Key == "ItemPerPage").Value;
            int itemPerPage = string.IsNullOrEmpty(itemPerPageStr) ? 3 : int.Parse(itemPerPageStr);
            return View(Pagination<Color>.Create(_context.Colors.Include(x => x.ShoeColorItems).AsQueryable(), page, itemPerPage));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Color color)
        {
            if (!ModelState.IsValid)
                return View();

            color.HexValue = "#" + color.HexValue;
            if (_context.Colors.Any(x => x.Name == color.Name || x.HexValue == color.HexValue))
            {
                ModelState.AddModelError("Name", "This Color Already Exists");
                return View();
            }

            _context.Colors.Add(color);
            _context.SaveChanges();

            return RedirectToAction("index");
        }


        public IActionResult Delete(int id)
        {

            Color color = _context.Colors.FirstOrDefault(x => x.Id == id);
            if (color == null)
            {
                return BadRequest();
            }
            _context.Remove(color);

            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            Color color = _context.Colors.FirstOrDefault(x => x.Id == id);
            if (color == null)
            {
                return BadRequest();
            }
            return View(color);
        }

        [HttpPost]
        public IActionResult Edit(Color color)
        {
            Color tempColor = _context.Colors.FirstOrDefault(x => x.Id == color.Id);
            if (tempColor.Name == color.Name || tempColor.HexValue == color.HexValue)
            {
                ModelState.AddModelError("Name", "This Color Already Exists");
                return View();
            }


            tempColor.Name = color.Name;
            tempColor.HexValue = color.HexValue;
            _context.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
