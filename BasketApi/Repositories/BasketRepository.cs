using BasketApi.Data;
using BasketApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<BasketItem>> GetMany(int customerId)
        {
            var items =  await _dataContext.BasketItems.Include(i => i.Item).Where(x => x.CustomerId == customerId).ToListAsync();
            return items;
        }

        public void Add(BasketItem item)
        {
            _dataContext.Add(item);
        }

        public async Task<bool> SaveAll()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }
    }
}
