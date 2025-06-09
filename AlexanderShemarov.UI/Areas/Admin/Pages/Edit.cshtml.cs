using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using AlexanderShemarov.Domain.Entities;
using AlexanderShemarov.UI.Services;


namespace AlexanderShemarov.UI.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly ITrainsService _trainsService;
        private readonly ITrainTypesService _trainTypesService;

        public EditModel(ITrainsService trainsService, ITrainTypesService trainTypesService)
        {
            _trainsService = trainsService;
            _trainTypesService = trainTypesService;
        }

        [BindProperty]
        public Trains Trains { get; set; } = default!;
        [BindProperty]
        public IFormFile? NewImage { get; set; }
        public SelectList TrainTypes { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _trainsService.GetTrainByIdAsync(id.Value);
            if (!response.Success)
            {
                return NotFound();
            }
            Trains = response.Data;

            var typesResponse = await _trainTypesService.GetTrainTypesListAsync();
            TrainTypes = new SelectList(typesResponse.Data, "ID", "Name");
            // ViewData["TrainTypesId"]
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var typesResponse = await _trainTypesService.GetTrainTypesListAsync();
                TrainTypes = new SelectList(typesResponse.Data, "ID", "Name");
                return Page();
            }

            var updateResult = await _trainsService.UpdateTrainAsync(Trains.ID, Trains, NewImage);
            if (!updateResult.Success)
            {
                ModelState.AddModelError("", updateResult.ErrorMessage);
                var typesResponse = await _trainTypesService.GetTrainTypesListAsync();
                TrainTypes = new SelectList(typesResponse.Data, "ID", "Name");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
