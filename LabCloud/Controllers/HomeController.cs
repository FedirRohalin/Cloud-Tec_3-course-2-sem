using LabCloud.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LabCloud.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult TriggerError()
        {
            // Цей рядок навмисно викидає виключення (Exception), 
            // що призведе до помилки 500 Internal Server Error
            throw new Exception("This is a test failure for Application Insights.");
        }
    }

}
