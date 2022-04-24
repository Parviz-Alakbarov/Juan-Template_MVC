using JuanShoesStore.Models;
using JuanShoesStore.Util;
using JuanShoesStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JuanShoesStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly JuanContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, JuanContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        #region Login

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(MemberLoginViewModel memberVM)
        {
            if (!ModelState.IsValid)
                return View();

            AppUser user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == memberVM.UserName && !x.IsAdmin);
            if (user == null)
            {
                ModelState.AddModelError("", "Username or Password is incorrect");
                return View();
            }
            var result = await _signInManager.PasswordSignInAsync(user, memberVM.Password, memberVM.RememberMe, true);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or Password is incorrect");
                return View();
            }

            return RedirectToAction("index", "home");
        }
        #endregion

        #region Register

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(MemberRegisterViewModel registerVM)
        {
            if (!ModelState.IsValid)
                return View();

            AppUser user = await _userManager.FindByNameAsync(registerVM.UserName);

            if (user != null)
            {
                ModelState.AddModelError("UserName", "UserName already exists!");
                return View();
            }

            user = new AppUser
            {
                UserName = registerVM.UserName,
                Email = registerVM.Email,
                IsAdmin = false,
                FullName = registerVM.FullName,
                CreatedAt = DateTime.UtcNow.AddHours(4),

            };

            var createResult = await _userManager.CreateAsync(user, registerVM.Password);
            if (!createResult.Succeeded)
            {
                foreach (var err in createResult.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
                return View();
            }

            var roleResult = await _userManager.AddToRoleAsync(user, "Member");
            if (!roleResult.Succeeded)
            {
                foreach (var err in roleResult.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
                return View();
            }
            user.CreatedAt = DateTime.UtcNow.AddHours(4);
            await _signInManager.SignInAsync(user, isPersistent: registerVM.RememberMe);

            return RedirectToAction("index", "home");
        }
        #endregion

        #region Profile & EditProfile

        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Profile()
        {
            AppUser user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);

            MemberProfileViewModel profileVM = new MemberProfileViewModel
            {
                ProfileUpdateVM = new MemberUpdateViewModel
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    FullName = user.FullName,
                    Country = user.Country,
                    City = user.City,
                    Address = user.Address,
                    Phone = user.PhoneNumber,
                },
                Orders = _context.Orders.Include(x => x.OrderItems).ThenInclude(x => x.Shoe).ThenInclude(x => x.Brand).Where(x => x.AppUserId == user.Id).ToList()
            };

            return View(profileVM);
        }







        [HttpPost]
        [Authorize(Roles = "Member")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(MemberUpdateViewModel memberUpdateVM)
        {
            MemberProfileViewModel profileVM = new MemberProfileViewModel
            {
                ProfileUpdateVM = memberUpdateVM
            };
            TempData["ProfileTab"] = "Account";
            if (!ModelState.IsValid)
            {
                return View("profile", profileVM);
            }

            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user.UserName != memberUpdateVM.UserName && _userManager.Users.Any(x => x.UserName == user.UserName))
            {
                ModelState.AddModelError("UserName", "UserName Already Exists!");
                return RedirectToAction("profile", profileVM);
            };
            if (user.Email != memberUpdateVM.Email && _userManager.Users.Any(x => x.UserName == user.UserName))
            {
                ModelState.AddModelError("Email", "UserName Already Exists!");
                return RedirectToAction("profile", profileVM);
            };

            user.UserName = memberUpdateVM.UserName;
            user.FullName = memberUpdateVM.FullName;
            user.Email = memberUpdateVM.Email;
            user.PhoneNumber = memberUpdateVM.Phone;
            user.Country = memberUpdateVM.Country;
            user.City = memberUpdateVM.City;
            user.Address = memberUpdateVM.Address;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (IdentityError err in updateResult.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
                return View("profile", profileVM);
            }

            if (memberUpdateVM.NewPassword != null)
            {
                if (string.IsNullOrWhiteSpace(memberUpdateVM.Password))
                {
                    ModelState.AddModelError("Password", "Current Password Is Required");
                    return View("profile", profileVM);
                }

                if (!await _userManager.CheckPasswordAsync(user, memberUpdateVM.Password))
                {
                    ModelState.AddModelError("Password", "Current Password isn't correct!!");
                    return View("profile", profileVM);
                }

                var ChangeResult = await _userManager.ChangePasswordAsync(user, memberUpdateVM.Password, memberUpdateVM.NewPassword);

                if (!ChangeResult.Succeeded)
                {
                    foreach (IdentityError err in ChangeResult.Errors)
                    {
                        ModelState.AddModelError("", err.Description);
                    }
                    return View("profile", profileVM);
                }
            }
            TempData["Success"] = "Profile updated successfully";
            return RedirectToAction("profile");
        }
        #endregion

        #region Logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("login");
        }
        #endregion

        #region ForgotPassword

        public IActionResult ForgotPassword()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return RedirectToAction("error", "home");

            AppUser user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return RedirectToAction("error", "home");

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);

            string link = Url.Action("ResetPassword", "Account", new { user.Id, token }, protocol: HttpContext.Request.Scheme);

            string text = System.IO.File.ReadAllText(Constants.TemplatesFolderPath + "/ResetPassword.html");
            text = text.Replace("[LoginLink]", Constants.AccountRegisterLink);
            text = text.Replace("[ResetPassword]", link);

            await SendEmailUtil.SendEmailUtilAsync(email, "reset", text);

            TempData["Success"] = "Check Your Email for Reset Password!";
            return RedirectToAction("login");

        }

        public async Task<IActionResult> ResetPassword(string id, string token)
        {
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(token))
                return BadRequest();

            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return RedirectToAction("error", "home");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(string id, string token, ResetPasswordViewModel resetVM)
        {
            if (!ModelState.IsValid)
                return View();

            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(token))
                return BadRequest();

            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return RedirectToAction("error", "home");

            var result = await _userManager.ResetPasswordAsync(user, token, resetVM.NewPassword);

            if (result.Errors != null)
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
                return View();
            }

            TempData["Success"] = "Password Changed Successfully";
            return RedirectToAction("login");
        }



        #endregion


    }
}
