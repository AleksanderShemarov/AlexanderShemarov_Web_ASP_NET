using Microsoft.AspNetCore.Mvc;


namespace AlexanderShemarov.UI.Components
{
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }

    }
}
