using AlexanderShemarov.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AlexanderShemarov.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public List<string> listElems;

        public HomeController(ILogger<HomeController> logger)
        {
            listElems = [
                "������� ������ � �������� 0",
                "������� ������ � �������� 1",
                "������� ������ � �������� 2",
                "������� ������ � �������� 3",
                "������� ������ � �������� 4",
            ];

            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(listElems);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
