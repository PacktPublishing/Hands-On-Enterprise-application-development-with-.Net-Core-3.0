using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Draken.Web.ViewComponents
{
    public class NavigationViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
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
