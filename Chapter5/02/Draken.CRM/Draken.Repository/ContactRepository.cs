using Draken.Models;
using Microsoft.Extensions.Configuration;

namespace Draken.Repository
{
    public interface IContactRepository : IGenericRepository<Contact>
    {
    }

    public class ContactRepository : GenericRepository<Contact>, IContactRepository
    {
        public ContactRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
