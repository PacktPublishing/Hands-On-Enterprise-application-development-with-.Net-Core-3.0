using Draken.Models;
using Draken.Service;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Draken.Web.Controllers
{
    public class ContactController : Controller
    {
        readonly IContactService contactService;

        public ContactController(IContactService contactService)
        {
            this.contactService = contactService;
        }

        public IActionResult Index()
        {
            var contacts = this.contactService.GetAll();
            return View(contacts);
        }

        public IActionResult Create()
        {
            return View(new Contact());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Contact contact)
        {
            try
            {
                this.contactService.Create(contact);
                return RedirectToAction(nameof(Index));
            }
            catch  (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }
    }
}
