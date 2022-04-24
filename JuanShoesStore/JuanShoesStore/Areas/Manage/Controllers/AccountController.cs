using JuanShoesStore.Areas.Manage.ViewModels;
using JuanShoesStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JuanShoesStore.Areas.Manage.Controllers
{
    [Area("manage")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginViewModel adminVM)
        {
            if (!ModelState.IsValid)
                return View();

            AppUser user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == adminVM.UserName );
            if (user == null)
            {
                ModelState.AddModelError("", "Username or Password is incorrect");
                return View();
            }
            var result = await _signInManager.PasswordSignInAsync(user, adminVM.Password, adminVM.RememberMe, true);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or Password is incorrect");
                return View();
            }

            return RedirectToAction("index", "dashboard");
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("login");
        }

        [Authorize(Roles = "SuperAdmin")]
        public IActionResult AddRole()
        {
           
            return View();
        }
        [HttpPost]
        [Authorize(Roles ="SuperAdmin")]
        public async Task<IActionResult> AddRole(AdminAddRoleViewModel adminRVM)
        {
            if (!ModelState.IsValid)
                return View();
           

            AppUser user = await _userManager.FindByNameAsync(adminRVM.UserName);

            if (user != null)
            {
                ModelState.AddModelError("UserName", "This UserName Already exists!");
                return View();
            }

            user = new AppUser
            {
                UserName = adminRVM.UserName,
                Email = adminRVM.Email,
                FullName = adminRVM.FullName,
                IsAdmin = true,
            };

            var createResult = await _userManager.CreateAsync(user, adminRVM.Password);

            if (!createResult.Succeeded)
            {
                foreach (var err in createResult.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }

            var ıdentityResult = await _userManager.AddToRoleAsync(user, "Admin");

            if (!ıdentityResult.Succeeded)
            {
                foreach (var err in ıdentityResult.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }

            return RedirectToAction("index", "dashboard");
        }
    }
}
