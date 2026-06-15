using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Frontend.Models;
using Frontend.Models.Entities;
using Frontend.Models.ViewModels;

namespace Frontend.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public AuthController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, isPersistent: true, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    return Redirect(model.ReturnUrl);

                var user = await _userManager.FindByNameAsync(model.Username);
                if (user != null)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Contains("Admin"))
                        return RedirectToAction("Dashboard", "Admin");
                    if (roles.Contains("Professor"))
                        return RedirectToAction("Dashboard", "Professor");
                    if (roles.Contains("Student"))
                        return RedirectToAction("Dashboard", "Student");
                }

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.Role == "Professor" || model.Role == "Admin")
            {
                if (string.IsNullOrWhiteSpace(model.SecretCode) || model.SecretCode != "DEC2026")
                {
                    ModelState.AddModelError("SecretCode", "Código de acesso institucional inválido.");
                    return View(model);
                }
            }

            ApplicationUser user;
            if (model.Role == "Admin")
            {
                user = new ApplicationUser();
                user.RoleId = 3;
            }
            else if (model.Role == "Professor")
            {
                user = new Professor();
                user.RoleId = 2;
            }
            else
            {
                user = new Student();
                user.RoleId = 1;
            }
            
            user.UserName = model.Username;
            user.Email = model.Email;
            user.FullName = model.FullName;
            user.UserStatusId = 1; 
            user.GenderId = 1; 

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {

                if (!await _roleManager.RoleExistsAsync(model.Role))
                {
                    await _roleManager.CreateAsync(new IdentityRole<int>(model.Role));
                }


                await _userManager.AddToRoleAsync(user, model.Role);

                TempData["SuccessMessage"] = "Registration successful! You can now log in.";
                return RedirectToAction("Login");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
