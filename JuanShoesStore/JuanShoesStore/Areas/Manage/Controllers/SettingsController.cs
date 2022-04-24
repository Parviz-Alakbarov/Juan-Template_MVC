using JuanShoesStore.Models;
using JuanShoesStore.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace JuanShoesStore.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "SuperAdmin")]
    public class SettingsController : Controller
    {
        JuanContext _context;

        public SettingsController(JuanContext context)
        {
            _context = context;
        }

        public IActionResult Index(int page = 1)
        {
            if (page < 1)
            {
                return RedirectToAction("error", "home");
            }

            string itemPerPageStr = _context.Setting.FirstOrDefault(x => x.Key == "ItemPerPage").Value;
            int itemPerPage = string.IsNullOrEmpty(itemPerPageStr) ? 3 : int.Parse(itemPerPageStr);

            return View(Pagination<Setting>.Create(_context.Setting.AsQueryable(), page, itemPerPage));
        }


        public IActionResult Edit(int id)
        {
            Setting setting = _context.Setting.FirstOrDefault(x => x.Id == id);
            if (setting == null) { return RedirectToAction("error", "home"); }

            return View(setting);

        }

        [HttpPost]
        public IActionResult EditWhichHasText(Setting setting)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", setting);
            }

            Setting existSettig = _context.Setting.FirstOrDefault(x => x.Key == setting.Key);

            if (existSettig == null) { RedirectToAction("error", "home"); }

            existSettig.Value = setting.Value;

            _context.SaveChanges();

            return RedirectToAction("index");

        }

        [HttpPost]
        public IActionResult EditWhichHasImage(Setting setting)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", setting);
            }

            Setting existSetting = _context.Setting.FirstOrDefault(x => x.Key == setting.Key);
            if (existSetting == null) { RedirectToAction("error", "home"); }


            if (setting.ImageFile != null)
            {

                if (!setting.ImageFile.IsValidSize(2097152))
                {
                    ModelState.AddModelError("ImageFile", "The File Size Is Too Much");
                    return View();
                }

                if (!setting.ImageFile.IsImage())
                {
                    ModelState.AddModelError("ImageFile", "File Format Must be .jpeg or .png!");
                    return View();
                }

                FileManager.Delete(Constants.UploadImagesFolderPath, "settings", existSetting.Value);
                existSetting.Value = FileManager.Save(Constants.UploadImagesFolderPath, "settings", setting.ImageFile);
                _context.SaveChanges();
            }

            return RedirectToAction("index");
        }
    }
}
