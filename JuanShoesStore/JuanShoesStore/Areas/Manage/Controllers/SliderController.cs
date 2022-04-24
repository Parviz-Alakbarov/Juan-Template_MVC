using JuanShoesStore.Models;
using JuanShoesStore.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace JuanShoesStore.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class SliderController : Controller
    {
        private readonly JuanContext _context;
        private readonly IWebHostEnvironment _enviroment;
        public SliderController(JuanContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _enviroment = environment;
        }
        public IActionResult Index(int page = 1)
        {

            if (page < 1)
            {
                return RedirectToAction("error", "home");
            }

            string itemPerPageStr = _context.Setting.FirstOrDefault(x => x.Key == "ItemPerPage").Value;
            int itemPerPage = string.IsNullOrEmpty(itemPerPageStr) ? 3 : int.Parse(itemPerPageStr);

            return View(Pagination<Slider>.Create(_context.Sliders.AsQueryable(), page, itemPerPage));

        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult Create(Slider slider)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (slider.FormFile == null)
            {
                ModelState.AddModelError("FormFile", "Slider Image is required!");
                return View();
            }

            if (!slider.FormFile.IsValidSize(2097152))
            {
                ModelState.AddModelError("ImageFile", "The File Size Is Too Much");
                return View();
            }

            if (!slider.FormFile.IsImage())
            {
                ModelState.AddModelError("ImageFile", "File Format Must be .jpeg or .png!");
                return View();
            }

            slider.Image = FileManager.Save(_enviroment.WebRootPath, "uploads/slider", slider.FormFile);

            _context.Sliders.Add(slider);
            _context.SaveChanges();

            TempData["Success"] = "Slider Added Successfully";


            return RedirectToAction("index");
        }


        public IActionResult Edit(int id)
        {
            Slider slider = _context.Sliders.FirstOrDefault(x => x.Id == id);
            if (slider == null)
            {
                return NotFound();
            }
            return View(slider);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult Edit(Slider slider)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            Slider existSlider = _context.Sliders.FirstOrDefault(x => x.Id == slider.Id);

            if (existSlider == null)
            {
                return NotFound();
            }

            /*if (slider.FormFile == null)
            {
                ModelState.AddModelError("FormFile", "Slider Image is required!");
                return View();
            }*/

            

            if (slider.FormFile!=null)
            {

                if (!slider.FormFile.IsValidSize(2097152))
                {
                    ModelState.AddModelError("ImageFile", "The File Size Is Too Much");
                    return View();
                }

                if (!slider.FormFile.IsImage())
                {
                    ModelState.AddModelError("ImageFile", "File Format Must be .jpeg or .png!");
                    return View();
                }

                FileManager.Delete(Constants.UploadImagesFolderPath, "slider", existSlider.Image);
                existSlider.Image = FileManager.Save(Constants.UploadImagesFolderPath, "slider", slider.FormFile);
            }

            existSlider.Title = slider.Title;
            existSlider.BtnText = slider.BtnText;
            existSlider.Subtitle = slider.Subtitle;
            existSlider.RedirectURL = slider.RedirectURL;
            existSlider.Desc = slider.Desc;
            _context.SaveChanges();

            return RedirectToAction("index");
        }


        
        public IActionResult Delete(int id)
        {
            Slider existSlider = _context.Sliders.FirstOrDefault(x => x.Id == id);

            if (existSlider==null)
            {
                return NotFound();
            }

            bool deleted = FileManager.Delete(Constants.UploadImagesFolderPath, "slider", existSlider.Image);
            if (!deleted)
            {
                return BadRequest();
            }

            _context.Remove(existSlider);
            _context.SaveChanges();

            return RedirectToAction("index");
        }


    }
}
