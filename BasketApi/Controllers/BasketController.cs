using BasketApi.Dtos;
using BasketApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketApi.Controllers
{
    [Route("/api/[controller]")]
    public class BasketController : Controller
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpGet("{customerId}", Name = "GetBasket")]
        public async Task<IActionResult> GetBasket (int customerId)
        {
            var basket = await _basketService.GetBasket(customerId);

            if (!basket.Items.Any())
            {
                return NoContent();
            }

            return Ok(basket);
        }

        [HttpPost]
        public async Task<IActionResult> AddItemToBasket(int customerId, ItemToAddDto item)
        {
            if (!_basketService.AddItemToBasket(customerId, item))
            {
                return BadRequest("Couldn't add item to the basktet");
            }

        }
    }
}
