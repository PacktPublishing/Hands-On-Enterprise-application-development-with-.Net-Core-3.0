using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EAD.CRM.Web.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EAD.CRM.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Test()
        {
            throw new NotImplementedException();
        }
    }
}
