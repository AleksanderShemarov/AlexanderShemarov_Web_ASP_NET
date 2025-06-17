using AlexanderShemarov.Domain.Entities;
using AlexanderShemarov.UI.Services;
using Microsoft.AspNetCore.Mvc;


namespace AlexanderShemarov.UI.Components
{
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cart = HttpContext.Session.Get<Cart>("cart");
            return View(cart);
        }

    }
}
