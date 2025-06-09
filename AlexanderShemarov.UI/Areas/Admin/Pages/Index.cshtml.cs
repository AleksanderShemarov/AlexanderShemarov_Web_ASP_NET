using Microsoft.AspNetCore.Mvc.RazorPages;
using AlexanderShemarov.Domain.Entities;
using AlexanderShemarov.UI.Services;
using Microsoft.AspNetCore.Authorization;


namespace AlexanderShemarov.UI.Areas.Admin.Pages
{
    [Authorize(Policy = "admin")]
    public class IndexModel : PageModel
    {
        private readonly ITrainsService _trainsService;
        private readonly ITrainTypesService _trainTypesService;

        public IndexModel(ITrainsService trainsService, ITrainTypesService trainTypesService)
        {
            _trainsService = trainsService;
            _trainTypesService = trainTypesService;
        }

        public List<Trains> Trains { get; set; } = default!;
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
        public List<TrainTypes> TrainTypes { get; set; } = default!;

        public async Task OnGetAsync(int? pageNo = 1)
        {
            var response = await _trainsService.GetTrainsListAsync(null, pageNo.Value);
            if (response.Success)
            {
                Trains = response.Data.Items;
                CurrentPage = response.Data.CurrentPage;
                TotalPages = response.Data.TotalPages;
            }
            var response2 = await _trainTypesService.GetTrainTypesListAsync();
            if (response2.Success)
            {
                TrainTypes = response2.Data;
            }
        }
    }
}
