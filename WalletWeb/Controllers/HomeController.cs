using Microsoft.AspNetCore.Mvc;

namespace UI.WalletWeb.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
