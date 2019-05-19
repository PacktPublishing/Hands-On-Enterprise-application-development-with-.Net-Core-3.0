﻿using System.Collections.Generic;
using Draken.Service.Interfaces;
using Draken.Domain.Entities;
using Draken.Repository.Interfaces;

namespace Draken.Service
{
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