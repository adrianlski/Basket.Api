using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BasketApi.Models;

namespace BasketApi.Services
{
    public class BasketService : IBasketService
    {
        public async Task<List<BasketItem>> GetBasket(int customerId)
        {
            return new List<BasketItem>();
        }
    }
}
