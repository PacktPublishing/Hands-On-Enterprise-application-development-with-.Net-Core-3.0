using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Draken.Web.Models;
using Draken.Web.Services;
using Microsoft.AspNetCore.Authorization;

namespace Draken.Web.Controllers
{

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult SecurePage()
        {
            return View();
        }

        [Authorize(Policy = "SalesOnly")]
        public IActionResult ChangeToOpportunity()
        {
            return View();
        }

        [Authorize(Policy = "SalesChampionsOnly")]
        public IActionResult VacationPlanner()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("/error/{statusCode}")]
        public IActionResult ErrorPage([FromRoute]int statusCode)
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                ErrorCode = statusCode
            });
        }
    }
}
