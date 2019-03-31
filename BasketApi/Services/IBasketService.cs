using BasketApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketApi.Services
{
    public interface IBasketService
    {
        Task<List<BasketItem>> GetBasket(int customerId);
    }
}
