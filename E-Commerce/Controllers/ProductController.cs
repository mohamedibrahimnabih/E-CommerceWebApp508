﻿using E_Commerce.Data;
using E_Commerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Controllers
{
    public class ProductController : Controller
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();

        public IActionResult Index()
        {
            var products = dbContext.Products.Include(e=>e.Category).ToList();
            return View(products);
        }

        public IActionResult Create()
        {
            var categories = dbContext.Categories.ToList();

            return View(categories);
        }

        [HttpPost]
        public IActionResult Create(Product product, IFormFile ImgUrl) // "1.jpg"
        {
            if (ImgUrl.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImgUrl.FileName); // ".jpg"
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    ImgUrl.CopyTo(stream);
                }

                product.ImgUrl = fileName;
            }

            dbContext.Products.Add(product);
            dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int productId)
        {
            var product = dbContext.Products.Find(productId);

            var categories = dbContext.Categories.ToList();

            //ViewData["allCategories"] = categories;
            ViewBag.allCategories = categories;

            if (product != null)
                return View(product);

            return RedirectToAction("NotFoundPage", "Home");
        }

        [HttpPost]
        public IActionResult Edit(Product product, IFormFile ImgUrl) // "1.jpg"
        {
            var oldProduct = dbContext.Products.AsNoTracking().FirstOrDefault(e=>e.Id == product.Id);
            if (ImgUrl != null && ImgUrl.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImgUrl.FileName); // ".jpg"
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);
                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", oldProduct.ImgUrl);

                using (var stream = System.IO.File.Create(filePath))
                {
                    ImgUrl.CopyTo(stream);
                }

                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }

                product.ImgUrl = fileName;
            }
            else
            {
                product.ImgUrl = oldProduct.ImgUrl;
            }

            dbContext.Products.Update(product);
            dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int productId)
        {
            var oldProduct = dbContext.Products.AsNoTracking().FirstOrDefault(e => e.Id == productId);
            var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", oldProduct.ImgUrl);

            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);
            }

            Product product = new Product() { Id = productId };
            dbContext.Products.Remove(product);
            dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
