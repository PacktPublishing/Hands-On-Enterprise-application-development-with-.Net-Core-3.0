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
        public IList<Contact> GetAll()
        {
            var contacts = new List<Contact>
            {
                new Contact() { FirstName = "Andrew", LastName = "Mathew", Email = "andrew@hotmail.com" },
                new Contact() { FirstName = "John", LastName = "Mathew", Email = "andrew@hotmail.com" },
                new Contact() { FirstName = "Rob", LastName = "Mathew", Email = "andrew@hotmail.com" }
            };

            return contacts;
        }
    }
}
