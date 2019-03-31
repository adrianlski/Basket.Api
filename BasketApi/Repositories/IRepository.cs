using System.Collections.Generic;
using System.Threading.Tasks;

namespace BasketApi.Repositories
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetMany(int id);
    }
}
