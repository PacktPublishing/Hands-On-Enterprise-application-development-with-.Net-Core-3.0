using Draken.Models;
using System.Collections.Generic;

namespace Draken.Service.Interfaces
{
    public interface IContactService
    {
        IEnumerable<Contact> GetAll();
        Contact Get(int id);
        void Add(Contact contact);
        void Delete(int id);
        void Update(Contact contact);
    }
}
