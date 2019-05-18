using Draken.Models;
using Microsoft.Extensions.Configuration;

namespace Draken.Repository
{
    public interface IUserRepository : IGenericRepository<User>
    {
    }

    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
