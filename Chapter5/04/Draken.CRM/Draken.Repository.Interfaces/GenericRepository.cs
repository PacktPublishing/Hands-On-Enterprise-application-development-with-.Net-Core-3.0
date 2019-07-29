using Draken.Models;
using System.Collections.Generic;

namespace Draken.Repository.Interfaces
{
    public interface IGenericRepository<T> where T : Model
    {
        T Get(int id);
        IEnumerable<T> GetAll();
        void Create(T model);
        void Delete(int id);
        void Update(T model);
    }
}
