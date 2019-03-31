using BasketApi.Controllers;
using BasketApi.Models;
using BasketApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BasketApi.Tests.Unit.Controllers
{
    [TestFixture]
    public class BasketControllerUnitTest
    {
        private BasketController _sut;
        private Randomizer _randomizer;
        private Mock<IBasketService> _basketServiceMock;
        private const int MAX_VALUE = 1000;

        [SetUp]
        public void SetUp()
        {
            _basketServiceMock = new Mock<IBasketService>();
            _randomizer = new Randomizer(Guid.NewGuid().GetHashCode());
            _sut = new BasketController(_basketServiceMock.Object);
        }

        [Test]
        public async Task GetBasketCallsBasketServiceWithCustomerId()
        {
            // Arrange
            var customerId = _randomizer.Next(MAX_VALUE);
            var basket = GetBasket(customerId);
            _basketServiceMock.Setup(x => x.GetBasket(customerId)).ReturnsAsync(basket);

            // Act
            await _sut.GetBasket(customerId);

            // Assert
            _basketServiceMock.Verify(x => x.GetBasket(customerId), Times.Once);
        }

        [Test]
        public async Task GetBasketReturnsOkResult()
        {
            // Arrange
            var customerId = _randomizer.Next(MAX_VALUE);
            var basket = GetBasket(customerId);
            _basketServiceMock.Setup(x => x.GetBasket(customerId)).ReturnsAsync(basket);

            // Act
            var result = await _sut.GetBasket(customerId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetBasketReturnsNoContentIfNoItemsInBasket()
        {
            // Arrange
            var customerId = _randomizer.Next(MAX_VALUE);
            var basket = GetEmptyBasket();
            _basketServiceMock.Setup(x => x.GetBasket(customerId)).ReturnsAsync(basket);

            // Act
            var result = await _sut.GetBasket(customerId);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task GetBasketReturnsBasket()
        {
            // Arrange
            var customerId = _randomizer.Next(MAX_VALUE);
            var basket = GetBasket(customerId);
            _basketServiceMock.Setup(x => x.GetBasket(customerId)).ReturnsAsync(basket);

            // Act
            var result = await _sut.GetBasket(customerId);
            var okResult = result as OkObjectResult;

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        private List<BasketItem> GetBasket(int customerId)
        {
            return new List<BasketItem> { new BasketItem { CustomerId = customerId } };
        }

        private List<BasketItem> GetEmptyBasket()
        {
            return new List<BasketItem>();
        }
    }
}
