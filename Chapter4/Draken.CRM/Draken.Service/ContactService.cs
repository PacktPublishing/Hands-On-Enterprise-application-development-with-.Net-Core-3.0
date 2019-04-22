using System.Collections.Generic;
using Draken.Models;

namespace Draken.Service
{
    public interface IContactService
    {
        IList<Contact> GetAll();
    }

    public class ContactService : IContactService
    {
        readonly IContactService contactService;

        public ContactService(IContactService contactService)
        {
            this.contactService = contactService;
        }

        public IList<Contact> GetAll()
        {
            return contactService.GetAll();
        }
    }
}
