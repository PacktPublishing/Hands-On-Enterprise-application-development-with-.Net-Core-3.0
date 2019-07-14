using Microsoft.AspNetCore.Mvc;
namespace Draken.API.Controllers
{
    public class SpaController : Controller
    {
        public IActionResult SpaFallback()
        {
            return File("~/index.html", "text/html");
        }
    }
}
