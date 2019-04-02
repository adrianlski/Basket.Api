using BasketApi.Data;
using BasketApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BasketApi.Repositories
{
    public class BasketRepository : IRepository<BasketItem>
    {
        private readonly DataContext _dataContext;

        public BasketRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }


        public void Add(BasketItem item)
        {
            _dataContext.Add(item);
        }

        public async Task<IEnumerable<BasketItem>> GetManyAsync(Expression<Func<BasketItem, bool>> predicate)
        {
            return await _dataContext.BasketItems.Include(i => i.Item).Where(predicate).ToListAsync();
        }

        public async Task<BasketItem> GetOneAsync(Expression<Func<BasketItem, bool>> predicate)
        {
            return await  _dataContext.BasketItems.Include(i => i.Item).Where(predicate).SingleOrDefaultAsync();
        }

        public async Task<bool> ExistsAsync(Expression<Func<BasketItem, bool>> predicate)
        {
            if (await _dataContext.BasketItems.AnyAsync(predicate))
            {
                return true;
            }
                
            return false;
        }

        public void DeleteOne (BasketItem entity)
        {
            _dataContext.BasketItems.Remove(entity);
        }

        public void DeleteRange(IEnumerable<BasketItem> items)
        {
           _dataContext.BasketItems.RemoveRange(items);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }
    }
}
