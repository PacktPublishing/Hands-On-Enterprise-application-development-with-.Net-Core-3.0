using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Draken.Web.ViewComponents
{
    public class NavigationViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var menus = new Dictionary<string, string>
            {
                { "Home", "Index" },
                { "Privacy", "Privacy" }
            };

            return View(menus);
        }
    }
}
