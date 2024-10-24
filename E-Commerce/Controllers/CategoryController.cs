using E_Commerce.Data;
using E_Commerce.Models;
using E_Commerce.Repository;
using E_Commerce.Repository.IRepository;
using E_Commerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Controllers
{
    [Authorize(Roles = $"{SD.adminRole},{SD.CompanyRole}")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository categoryRepository;

        //ApplicationDbContext dbContext = new ApplicationDbContext(); 

        //CategoryRepository categoryRepository = new CategoryRepository();

        public CategoryController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        //[AllowAnonymous]
        public IActionResult Index()
        {
            //var categories = dbContext.Categories.Include(e=>e.Products).ToList();
            var categories = categoryRepository.GetAll("Products");

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
                //dbContext.Categories.Add(category);
                //dbContext.SaveChanges();
                categoryRepository.Create(category);
                categoryRepository.Commit();

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
            //var categories = dbContext.Categories.Where(e=>e.Name == name).ToList();
            //var category = dbContext.Categories.Find(categoryId);
            var category = categoryRepository.GetOne(e=>e.Id==categoryId);
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
                categoryRepository.Edit(category);
                categoryRepository.Commit();

                TempData["success"] = "Update category successfully";

                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        public IActionResult Delete(int categoryId)
        {
            Category category = new Category() { Id = categoryId };
            categoryRepository.Delete(category);
            categoryRepository.Commit();

            TempData["success"] = "Delete category successfully";

            return RedirectToAction(nameof(Index));
        }
    }
}
