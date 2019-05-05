using Draken.Service;
using Microsoft.AspNetCore.Mvc;

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
    }
}
