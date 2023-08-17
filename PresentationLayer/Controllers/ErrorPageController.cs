using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    public class ErrorPageController : Controller
    {
        public IActionResult Page404()
        {
            return View();
        }
    }
}
