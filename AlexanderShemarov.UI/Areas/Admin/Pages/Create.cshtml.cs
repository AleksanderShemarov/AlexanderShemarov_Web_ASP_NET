using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using AlexanderShemarov.Domain.Entities;
using AlexanderShemarov.UI.Services;


namespace AlexanderShemarov.UI.Areas.Admin.Pages
{
    public class CreateModel(ITrainTypesService trainTypesService, ITrainsService trainsService) : PageModel
    {
        public async Task<IActionResult> OnGet()
        {
            var trainTypesListData = await trainTypesService.GetTrainTypesListAsync();
            ViewData["TrainTypesId"] = new SelectList(trainTypesListData.Data, "ID", "Name");
            return Page();
        }

        [BindProperty]
        public Trains Trains { get; set; } = default!;
        [BindProperty]
        public IFormFile? Image {  get; set; }

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await trainsService.CreateTrainAsync(Trains, Image);

            return RedirectToPage("./Index");
        }
    }
}
