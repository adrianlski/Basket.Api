using BasketApi.Controllers;
using BasketApi.Dtos;
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
    public class BasketControllerUnitTest : TestBase
    {
        private BasketController _sut;
        
        private Mock<IBasketService> _basketServiceMock;
        

        [SetUp]
        public void SetUp()
        {
            _basketServiceMock = new Mock<IBasketService>();
            _sut = new BasketController(_basketServiceMock.Object);
        }

        [Test]
        public async Task GetBasketCallsBasketServiceWithCustomerId()
        {
            // Arrange
            var customerId = GetRandomInt();
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
            var customerId = GetRandomInt();
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
            var customerId = GetRandomInt();
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
            var customerId = GetRandomInt();
            var basket = GetBasket(customerId);
            _basketServiceMock.Setup(x => x.GetBasket(customerId)).ReturnsAsync(basket);

            // Act
            var result = await _sut.GetBasket(customerId);
            var okResult = result as OkObjectResult;

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task AddItemToBasketCallsBasketServiceWithCustomerIdAndItem()
        {
            // Arrange
            var customerId = GetRandomInt();
            var itemToAddDto = GetItemToAddDto();

            // Act
            await _sut.AddItemToBasket(customerId, itemToAddDto);

            // Assert
            _basketServiceMock.Verify(x => x.AddItemToBasket(customerId, itemToAddDto), Times.Once);
        }

        [Test]
        public async Task AddItemToBasketReturnsNoContentWhenAddingSuccesful()
        {
            // Arrange
            var customerId = GetRandomInt();
            var itemToAddDto = GetItemToAddDto();
            _basketServiceMock.Setup(x => x.AddItemToBasket(customerId, itemToAddDto)).ReturnsAsync(true);

            // Act
            var result = await _sut.AddItemToBasket(customerId, itemToAddDto);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task AddItemToBasketReturnsBadRequestWhenAddingUnsuccseful()
        {
            // Arrange
            var customerId = GetRandomInt();
            var itemToAddDto = GetItemToAddDto();
            _basketServiceMock.Setup(x => x.AddItemToBasket(customerId, itemToAddDto)).ReturnsAsync(false);

            // Act
            var result = await _sut.AddItemToBasket(customerId, itemToAddDto);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }


        private ItemToAddDto GetItemToAddDto()
        {
            return new ItemToAddDto
            {
                ItemId = GetRandomInt(),
                Quantity = GetRandomInt()
            };
        }

        private BasketToReturnDto GetBasket(int customerId)
        {
            return new BasketToReturnDto { CustomerId = customerId , Items = new List<ItemToReturnDto> { new ItemToReturnDto() } };
        }

        private BasketToReturnDto GetEmptyBasket()
        {
            return new BasketToReturnDto();
        }
    }
}
