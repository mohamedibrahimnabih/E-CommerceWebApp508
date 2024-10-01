using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    public class WelcomeController : Controller
    {
        public IActionResult Index()
        {
            string name = "Mohamed Nabih From Controller";
            List<string> list = new List<string>() { "Ahmed", "Ali", "Mona", "Mohamed" };

            return View(model: list);
        }
    }
}
