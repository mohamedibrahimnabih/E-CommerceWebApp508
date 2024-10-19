using E_Commerce.Models;
using E_Commerce.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        // Register
        public IActionResult Register()
        {
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
