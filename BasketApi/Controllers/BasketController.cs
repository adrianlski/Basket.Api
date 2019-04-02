using System;
using BasketApi.Dtos;
using BasketApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using BasketApi.Exceptions;

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
            if (itemToAddDto == null || !ModelState.IsValid)
            {
                return BadRequest("The item is invalid");
            }

            try
            {
                var result = await _basketService.AddItemToBasket(customerId, itemToAddDto);

                if (result)
                {
                    return Ok();
                }

                return BadRequest("Couldn't add item to the basket");
            }
            catch (ItemExistsInTheBasketException e)
            {
                return BadRequest("Item already exists in the basket");
            }
        }

        [HttpDelete("{customerId}/item/{itemId}")]
        public async Task<IActionResult> RemoveItemFromBasket(int customerId, int itemId)
        {
            try
            {
                var result = await _basketService.RemoveItemFromBasktet(customerId, itemId);

                if (result)
                {
                    return Ok();
                }

                return BadRequest("Couldn't remove item from the basket");
            }
            catch (ItemNotInTheBasketException e)
            {
                return BadRequest("Item not found in the basket");
            }
        }

        [HttpPut("{customerId}")]
        public async Task<IActionResult> UpdateItemInTheBasket(int customerId, [FromBody] ItemToUpdateDto itemToUpdateDto)
        {
            if (itemToUpdateDto == null || !ModelState.IsValid)
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
        public async Task<IActionResult> ClearBasket(int customerId)
        {
            if (await _basketService.ClearBasket(customerId))
            {
                return Ok();
            }

            return BadRequest("Couldn't update item in the basket");
        }
    }
}
