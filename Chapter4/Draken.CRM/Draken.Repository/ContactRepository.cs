using System.Collections.Generic;
using Draken.Models;

namespace Draken.Repository
{
    public interface IContactRepository
    {
        IList<Contact> GetAll();
    }

    public class ContactRepository : IContactRepository
    {
        public ContactRepository()
        {
        }

        public IList<Contact> GetAll()
        {
            var contacts = new List<Contact>
            {
                new Contact() { FirstName = "Andrew", LastName = "Mathew", Email = "andrew@hotmail.com" }
            };

            return contacts;
        }
    }

}
