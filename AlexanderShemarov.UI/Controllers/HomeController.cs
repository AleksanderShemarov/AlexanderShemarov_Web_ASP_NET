using AlexanderShemarov.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;


namespace AlexanderShemarov.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public List<string> listElems;
        private readonly List<ListDemo> _SelectList;

        public HomeController(ILogger<HomeController> logger)
        {
            listElems = [
                "элемент списка с индексом 0",
                "элемент списка с индексом 1",
                "элемент списка с индексом 2",
                "элемент списка с индексом 3",
                "элемент списка с индексом 4",
            ];

            _SelectList = [
                new ListDemo { Id = 1, Name = "Item 1" },
                new ListDemo { Id = 2, Name = "Item 2" },
                new ListDemo { Id = 3, Name = "Item 3" },
            ];

            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewData["text"] = "Лабораторная работа №7";
            ViewData["list"] = listElems;
            //ViewData["select"] = _SelectList;

            SelectList selectData = new SelectList(_SelectList, "Id", "Name");
            return View(selectData);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
