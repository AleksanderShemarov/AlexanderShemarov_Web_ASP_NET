using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AlexanderShemarov.Domain.Entities;
using AlexanderShemarov.UI.Services;


namespace AlexanderShemarov.UI.Areas.Admin.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly ITrainsService _trainsService;
        private readonly ITrainTypesService _trainTypesService;

        public DeleteModel(ITrainsService trainsService, ITrainTypesService trainTypesService)
        {
            _trainsService = trainsService;
            _trainTypesService = trainTypesService;
        }

        [BindProperty]
        public Trains Trains { get; set; } = default!;
        public List<TrainTypes> TrainTypes { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _trainsService.GetTrainByIdAsync(id.Value);
            if (!response.Success || response.Data == null)
            {
                return NotFound();
            }

            var response2 = await _trainTypesService.GetTrainTypesListAsync();
            if (response2.Success)
            {
                TrainTypes = response2.Data;
            }

            Trains = response.Data;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _trainsService.GetTrainByIdAsync(id.Value);
            if (!response.Success || response.Data == null)
            {
                return NotFound();
            }

            var deleteResult = await _trainsService.DeleteTrainAsync(id.Value);
            if (!deleteResult.Success)
            {
                ModelState.AddModelError(string.Empty, deleteResult.ErrorMessage ?? "Error during removing");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
