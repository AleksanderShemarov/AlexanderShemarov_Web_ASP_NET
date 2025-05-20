using AlexanderShemarov.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace AlexanderShemarov.UI.Controllers
{
    public class ProductController(ITrainTypesService trainTypesService, ITrainsService trainsService) : Controller
    {
        public async Task<IActionResult> Index(string? trainType)
        {
            var trainTypesResponse = await trainTypesService.GetTrainTypesListAsync();

            if(!trainTypesResponse.Success) return NotFound(trainTypesResponse.ErrorMessage);

            ViewData["trainTypes"] = trainTypesResponse.Data;

            var currentTrainType = trainType == null ? "All"
                : trainTypesResponse.Data.FirstOrDefault(
                    tt => tt.NormalizedName == trainType
                )?.Name;
            ViewData["currentTrainType"] = currentTrainType;

            var trainsResponse = await trainsService.GetTrainsListAsync(trainType);
            if (!trainsResponse.Success) ViewData["Error"] = trainsResponse.ErrorMessage;
            return View(trainsResponse.Data.Items);
        }
    }
}
