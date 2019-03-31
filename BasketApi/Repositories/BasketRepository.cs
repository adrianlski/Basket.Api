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
            return await _dataContext.BasketItems.Where(x => x.CustomerId == customerId).ToListAsync();
        }
    }
}
