using E_Commerce.Models;
using E_Commerce.Utility;
using E_Commerce.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace E_Commerce.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        // Register
        public async Task<IActionResult> Register()
        {
            if(roleManager.Roles.IsNullOrEmpty())
            {
                await roleManager.CreateAsync(new(SD.adminRole));
                await roleManager.CreateAsync(new(SD.CompanyRole));
                await roleManager.CreateAsync(new(SD.CustomerRole));
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(ApplicationUserVM userVM)
        {
            if(ModelState.IsValid)
            {
                ApplicationUser applicationUser = new()
                {
                    UserName = userVM.UserName,
                    Email = userVM.Email,
                    Address = userVM.Address
                };

                var result = await userManager.CreateAsync(applicationUser, userVM.Password);

                if(result.Succeeded)
                {
                    // add user with role => customer, compa
                    await userManager.AddToRoleAsync(applicationUser, SD.CustomerRole);
                    await signInManager.SignInAsync(applicationUser, false);
                    return RedirectToAction("Index", "Home");
                }

                // error in password
                ModelState.AddModelError("Password", "Invalid password");
            }

            return View(userVM);
        }
        
        // Login
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if(ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(loginVM.UserName);

                if(user != null)
                {
                    var finalResult = await userManager.CheckPasswordAsync(user, loginVM.Password);

                    if(finalResult)
                    {
                        // Create ID
                        await signInManager.SignInAsync(user, loginVM.RemeberMe);
                        return RedirectToAction("Index", "Home");
                    }

                    // passwords incorrect
                    ModelState.AddModelError("Password", "invalid Password");
                }
                else
                {
                    // user not found
                    ModelState.AddModelError("UserName", "invalid user");
                }
                
            }
            return View(loginVM);
        }

        // Logout
        public IActionResult Logout()
        {
            signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
