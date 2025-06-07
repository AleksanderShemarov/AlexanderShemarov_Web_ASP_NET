using AlexanderShemarov.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace AlexanderShemarov.UI.Controllers
{
    public class ProductController(ITrainTypesService trainTypesService, ITrainsService trainsService) : Controller
    {
        [Route("Catalog")]
        [Route("Catalog/{trainType}")]
        public async Task<IActionResult> Index(string? trainType, int pageNo = 1)
        {
            var trainTypesResponse = await trainTypesService.GetTrainTypesListAsync();

            if(!trainTypesResponse.Success) return NotFound(trainTypesResponse.ErrorMessage);

            ViewData["trainTypes"] = trainTypesResponse.Data;

            var currentTrainType = trainType == null ? "All"
                : trainTypesResponse.Data.FirstOrDefault(
                    tt => tt.NormalizedName == trainType
                )?.Name;
            ViewData["currentTrainType"] = currentTrainType;

            var trainsResponse = await trainsService.GetTrainsListAsync(trainType, pageNo);
            if (!trainsResponse.Success) ViewData["Error"] = trainsResponse.ErrorMessage;
            
            return View(trainsResponse.Data);
        }
    }
}
