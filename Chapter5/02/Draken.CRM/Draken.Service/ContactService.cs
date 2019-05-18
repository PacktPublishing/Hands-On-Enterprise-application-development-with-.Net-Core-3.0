﻿using System.Collections.Generic;
using Draken.Models;
using Draken.Repository;

namespace Draken.Service
{
    public interface IContactService
    {
        IEnumerable<Contact> GetAll();
        Contact Get(int id);
        void Add(Contact contact);
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

        public void Add(Contact contact)
        {
            contact.CreatedDate = contact.ModifiedDate = System.DateTime.Now;
            contact.CreatedBy = contact.ModifiedBy = "System";

            contactRepository.Add(contact);
        }

        public void Delete(int id)
        {
            contactRepository.Delete(id);
        }

        public Contact Get(int id)
        {
            return contactRepository.Get(id);
        }

        public IEnumerable<Contact> GetAll()
        {
            return contactRepository.GetAll();
        }

        public void Update(Contact contact)
        {
            contact.ModifiedDate = System.DateTime.Now;
            contact.ModifiedBy = "System";

            contactRepository.Update(contact);
        }
    }
}
