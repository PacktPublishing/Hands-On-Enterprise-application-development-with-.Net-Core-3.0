using System.Collections.Generic;
using Draken.Models;
using Draken.Repository;

namespace Draken.Service
{
    public interface IContactService
    {
        IEnumerable<Contact> GetAll();
        Contact Get(int id);
        void Create(Contact contact);
        void Delete(int id);
        void Update(Contact contact);
    }

    public class ContactService : IContactService
    {
        private readonly IContactRepository contactRepository;

        public ContactService(IContactRepository contactRepository)
        {
            this.contactRepository = contactRepository;
        }

        public void Create(Contact contact)
        {
            contactRepository.Create(contact);
        }

        public void Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public Contact Get(int id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Contact> GetAll()
        {
            return contactRepository.GetAll();
        }

        public void Update(Contact contact)
        {
            throw new System.NotImplementedException();
        }
    }
}
