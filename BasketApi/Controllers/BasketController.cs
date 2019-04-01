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
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpGet("{customerId}", Name = "GetBasket")]
        public async Task<IActionResult> GetBasket(int customerId)
        {
            var basket = await _basketService.GetBasket(customerId);

            if (!basket.Items.Any())
            {
                return NoContent();
            }

            return Ok(basket);
        }

        [HttpPost("{customerId}")]
        public async Task<IActionResult> AddItemToBasket(int customerId, [FromBody] ItemToAddDto itemToAddDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("The item is invalid");
            }

            if (await _basketService.AddItemToBasket(customerId, itemToAddDto))
            {
                return NoContent();
            }

            return BadRequest("Couldn't add item to the basket");
        }

        [HttpPut("{customerId}")]
        public async Task<IActionResult> UpdateItemInTheBasket(int customerId, [FromBody] ItemToUpdateDto itemToUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("The item is invalid");
            }

            if (await _basketService.UpdateBasketItem(customerId, itemToUpdateDto))
            {
                return NoContent();
            }

            return BadRequest("Couldn't update item in the basket");

        }

        [HttpDelete("{customerId}")]
        public async Task<IActionResult> DeleteBasket(int customerId)
        {
            return Ok();
        }
    }
}
