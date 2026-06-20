using Microsoft.AspNetCore.Mvc;

namespace LabCloud.Controllers
{
    public class DeployTestController : Controller
    {
        // Цей метод обробляє запит за адресою /DeployTest/Index
        public IActionResult Index()
        {
            // Формуємо тестові дані для відображення на сторінці
            ViewBag.EnvironmentInfo = "Development (Готово до міграції в Azure)";
            ViewBag.AppVersion = "1.0.3-cloud-ready";
            ViewBag.Status = "Успішно скомпільовано";

            // Передаємо управління у відповідний файл View (Index.cshtml)
            return View();
        }
    }
}