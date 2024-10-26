using E_Commerce.Data;
using E_Commerce.Models;
using E_Commerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;
using static System.Net.WebRequestMethods;

namespace E_Commerce.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        ApplicationDbContext dbContext = new();
        private readonly UserManager<ApplicationUser> userManager;

        public ShoppingCartController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public IActionResult AddToCart(int productId, int count)
        {
            var applicationUserId = userManager.GetUserId(User);

            ShoppingCart shoppingCart = new ShoppingCart()
            {
                ProductId = productId,
                Count = count,
                ApplicationUserId = applicationUserId
            };

            var item = dbContext.ShoppingCarts.Include(e => e.Product).FirstOrDefault(e => e.ApplicationUserId == applicationUserId && e.ProductId == productId);

            if(item != null)
                item.Count += count;
            else
                dbContext.ShoppingCarts.Add(shoppingCart);

            dbContext.SaveChanges();

            TempData["success"] = "Add product to cart successfully";

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Index()
        {
            var applicationUserId = userManager.GetUserId(User);

            var ShoppingCarts = dbContext.ShoppingCarts.Include(e=>e.Product).Where(e=>e.ApplicationUserId == applicationUserId).ToList();
            ViewBag.TotalPrice = dbContext.ShoppingCarts.Where(e => e.ApplicationUserId == applicationUserId).Sum(e=>e.Product.Price * e.Count);

            return View(ShoppingCarts);
        }

        public IActionResult Increment(int productId)
        {
            var applicationUserId = userManager.GetUserId(User);

            var item = dbContext.ShoppingCarts.Include(e => e.Product).FirstOrDefault(e => e.ApplicationUserId == applicationUserId && e.ProductId == productId);

            if (item != null)
                item.Count++;

            dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Decremnt(int productId)
        {
            var applicationUserId = userManager.GetUserId(User);

            var item = dbContext.ShoppingCarts.Include(e => e.Product).FirstOrDefault(e => e.ApplicationUserId == applicationUserId && e.ProductId == productId);

            if (item != null)
                item.Count--;

            if (item.Count <= 0)
                item.Count = 1;

            dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Pay()
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = $"{Request.Scheme}://{Request.Host}/checkout/success",
                CancelUrl = $"{Request.Scheme}://{Request.Host}/checkout/cancel",
            };

            var applicationUserId = userManager.GetUserId(User);

            var ShoppingCarts = dbContext.ShoppingCarts.Include(e => e.Product).Where(e => e.ApplicationUserId == applicationUserId).ToList();

            foreach (var item  in ShoppingCarts)
            {
                options.LineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "egp",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name,
                            Description = item.Product.Description,
                        },
                        UnitAmount = (long)item.Product.Price*100,
                    },
                    Quantity = item.Count,
                });
            }

            var service = new SessionService();
            var session = service.Create(options);
            return Redirect(session.Url);
        }
    }
}
