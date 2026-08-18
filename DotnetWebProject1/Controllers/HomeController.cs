using Microsoft.AspNetCore.Mvc;

namespace DotnetWebProject1.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
    }
}