using System;
using Draken.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Draken.Web.Controllers
{
    public class ContactController : Controller
    {
        readonly IContactRepository contactRepository;

        public ContactController(IContactRepository contactRepository)
        {
            this.contactRepository = contactRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
