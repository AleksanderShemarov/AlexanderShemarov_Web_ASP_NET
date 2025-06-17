using AlexanderShemarov.Domain.Entities;
using AlexanderShemarov.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace AlexanderShemarov.UI.Controllers
{
    public class CartController : Controller
    {
        private readonly ITrainsService _trainsService;
        private Cart _cart;

        public CartController(ITrainsService trainsService)
        {
            _trainsService = trainsService;
        }

        // GET: CartController
        public IActionResult Index()
        {
            _cart = HttpContext.Session.Get<Cart>("cart") ?? new();
            return View(_cart.CartItems);
        }

        [Route("[controller]/add/{id:int}")]
        public async Task<ActionResult> Add(int id, string returnUrl)
        {
            var data = await _trainsService.GetTrainByIdAsync(id);
            if (data.Success)
            {
                _cart = HttpContext.Session.Get<Cart>("cart") ?? new();
                _cart.AddToCart(data.Data);
                HttpContext.Session.Set<Cart>("cart", _cart);
            }

            return Redirect(returnUrl);
        }

        [Route("[controller]/remove/{id:int}")]
        public ActionResult Remove(int id)
        {
            _cart = HttpContext.Session.Get<Cart>("cart") ?? new();
            _cart.RemoveItems(id);
            HttpContext.Session.Set<Cart>("cart", _cart);
            return RedirectToAction("Index");
        }
    }
}
