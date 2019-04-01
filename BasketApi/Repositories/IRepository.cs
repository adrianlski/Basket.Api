using System.Collections.Generic;
using System.Threading.Tasks;
using BasketApi.Dtos;

namespace BasketApi.Repositories
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetMany(int id);
        void Add(T item);
        Task<bool> SaveAll();
    }
}
