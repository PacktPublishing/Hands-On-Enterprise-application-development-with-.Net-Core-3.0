using Draken.Models;
using Draken.Repository.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Draken.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
