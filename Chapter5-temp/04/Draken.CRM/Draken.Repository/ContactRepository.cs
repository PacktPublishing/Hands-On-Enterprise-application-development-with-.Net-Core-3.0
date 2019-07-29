using Draken.Models;
using Draken.Repository.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Draken.Repository
{
  
    public class ContactRepository : GenericRepository<Contact>, IContactRepository
    {
        public ContactRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
