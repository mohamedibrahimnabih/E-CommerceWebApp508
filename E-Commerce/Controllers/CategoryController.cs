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

            //ViewBag.success = TempData["success"];
            //ViewBag.success = Request.Cookies["success"];

            return View(model: categories);
        }

        public IActionResult Create()
        {
            Category category = new Category(); 
            return View(category);
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if(ModelState.IsValid)
            {
                //Category category = new Category() { Name = categoryName };
                dbContext.Categories.Add(category);
                dbContext.SaveChanges();

                //CookieOptions options = new CookieOptions();
                //options.Secure = true;
                //options.Expires = DateTimeOffset.Now.AddHours(14);

                TempData["success"] = "Add category successfully";
                //Response.Cookies.Append("success", "Add category successfully", options);

                return RedirectToAction(nameof(Index));
            }

            return View(category);
            
        }

        public IActionResult Edit(int categoryId)
        {
            var category = dbContext.Categories.Find(categoryId);
            if(category != null)
                return View(category);

            return RedirectToAction("NotFoundPage", "Home");
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if(ModelState.IsValid)
            {
                //Category category = new Category() { Name = categoryName };
                dbContext.Categories.Update(category);
                dbContext.SaveChanges();

                TempData["success"] = "Update category successfully";

                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        public IActionResult Delete(int categoryId)
        {
            Category category = new Category() { Id = categoryId };
            dbContext.Categories.Remove(category);
            dbContext.SaveChanges();

            TempData["success"] = "Delete category successfully";

            return RedirectToAction(nameof(Index));
        }
    }
}
