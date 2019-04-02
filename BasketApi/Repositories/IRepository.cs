using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BasketApi.Repositories
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> predicate);
        void Add(T item);
        Task<bool> SaveAllAsync();
        Task<T> GetOneAsync(Expression<Func<T, bool>> predicate);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
        void DeleteOne(T entity);
        void DeleteRange(IEnumerable<T> items);
    }
}
