using BasketApi.Dtos;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BasketApi.Tests.Integration.Controllers
{
    [TestFixture]
    public class BasketControllerIntegrationTests
    {
        private HttpClient _client;
        private const string BASE_ADDRESS = "/api/basket";

        [SetUp]
        public void SetUp()
        {
            _client = new WebApplicationFactory<Startup>().CreateClient();
        }

        [Test]
        public async Task GetBasketReturnsNoContentWhenNoBasketFound()
        {
            // Arrange
            var noExistentBasketId = "2";

            // Act
            var result = await _client.GetAsync($"{BASE_ADDRESS}/{noExistentBasketId}");

            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, result.StatusCode);

        }

        [Test]
        public async Task AddItemToBasketReturnsBadRequestWhenStateIsNotValid()
        {
            // Arrange
            var basketId = 1;
            var itemId = 1;
            var invalidQuantity = -1;
            var itemToAddDto = GetItemToAddDto(itemId, invalidQuantity);
            var content = new StringContent(itemToAddDto.ToString(), Encoding.UTF8, "application/json");

            // Act
            var result = await _client.PostAsync($"{BASE_ADDRESS}/{basketId}", content);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Test]
        public async Task AddItemToBasketReturnsOKIfInsertingValidItem()
        {
            // Arrange
            var basketId = 1;
            var validId = 2;
            var quantity = 1;
            var itemToAddDto = GetItemToAddDto(validId, quantity);
            var content = new StringContent(JsonConvert.SerializeObject(itemToAddDto), Encoding.UTF8, "application/json");

            // Act
            var result = await _client.PostAsync($"{BASE_ADDRESS}/{basketId}", content);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        private ItemToAddDto GetItemToAddDto(int id ,int quantity)
        {
            return new ItemToAddDto
            {
                ItemId = id,
                Quantity = quantity
            };
        }
    }
}
