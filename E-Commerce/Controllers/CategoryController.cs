using E_Commerce.Data;
using E_Commerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    public class CategoryController : Controller
    {
        ApplicationDbContext dbContext = new ApplicationDbContext(); 

        public IActionResult Index()
        {
            var categories = dbContext.Categories.ToList();

            return View(model: categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            //Category category = new Category() { Name = categoryName };
            dbContext.Categories.Add(category);
            dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
