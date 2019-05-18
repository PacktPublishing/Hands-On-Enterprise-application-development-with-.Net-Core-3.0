using System.Collections.Generic;
using Draken.Domain.Entities;

namespace Draken.Repository.Interfaces
{
    public interface IGenericRepository<T> where T : Entity
    {
        T Get(int id);
        IEnumerable<T> GetAll();
        void Add(T model);
        void Delete(int id);
        void Update(T model);
    }
}
