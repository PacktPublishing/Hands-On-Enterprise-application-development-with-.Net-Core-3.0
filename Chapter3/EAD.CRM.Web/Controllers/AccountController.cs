using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EAD.CRM.Web.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            ViewData["ReturnUrl"] = Request.Query["ReturnUrl"];
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(IFormCollection formCollection, string returnUrl = null)
        {
            var username = formCollection["Username"].First().ToString();
            var password = formCollection["Password"].First().ToString();
            if (username.Equals("crmuser", StringComparison.OrdinalIgnoreCase) && password.Equals("Password@123", StringComparison.OrdinalIgnoreCase))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role,"Administrator"),
                    new Claim("NumberOfAwards","26")
                };
                
                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));
                return string.IsNullOrEmpty(returnUrl) ? Redirect("/") : Redirect(returnUrl);
            }
            else
            {
                ViewBag.Message = "Login failed. Please check your credentials";
            }

            return View();
        }
    }
}
