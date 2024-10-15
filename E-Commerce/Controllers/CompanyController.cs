using E_Commerce.Data;
using E_Commerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Controllers
{
    public class CompanyController : Controller
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();

        public IActionResult Index()
        {
            var companies = dbContext.Companies.Include(e=>e.Products).ToList();

            return View(companies);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Company company)
        {
            if (ModelState.IsValid)
            {
                dbContext.Companies.Add(company);
                dbContext.SaveChanges();
                TempData["success"] = "Add company successfully";

                return RedirectToAction(nameof(Index));
            }

            return View(company);
        }

        public IActionResult Edit(int companyId)
        {
            var company = dbContext.Companies.Find(companyId);
            if (company != null)
                return View(company);

            return RedirectToAction("NotFoundPage", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Company company)
        {
            if (ModelState.IsValid)
            {
                dbContext.Companies.Update(company);
                dbContext.SaveChanges();
                TempData["success"] = "Update company successfully";

                return RedirectToAction(nameof(Index));
            }

            return View(company);
        }
    }
}
