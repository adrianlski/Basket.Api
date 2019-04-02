using BasketApi.Controllers;
using BasketApi.Dtos;
using BasketApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using BasketApi.Exceptions;

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
        public async Task AddItemToBasketReturnsBadRequestIfItemIsNull()
        {
            // Arrange
            var customerId = GetRandomInt();

            // Act
            var result = await _sut.AddItemToBasket(customerId, null);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task AddItemToBasketReturnsBadRequestIfItemIsIsInvalid()
        {
            // Arrange
            var customerId = GetRandomInt();
            var itemToAddDto = GetItemToAddDto(quantity: -1);

            // Act
            var result = await _sut.AddItemToBasket(customerId, itemToAddDto);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task AddItemReturnsBadRequestIfItemAlreadyExistsInBasket()
        {
            // Arrange
            var customerId = GetRandomInt();
            var itemToAddDto = GetItemToAddDto();
            _basketServiceMock.Setup(x => x.AddItemToBasket(customerId, itemToAddDto)).ThrowsAsync(new ItemExistsInTheBasketException());

            // Act
            var result = await _sut.AddItemToBasket(customerId, itemToAddDto);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
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
        public async Task AddItemToBasketReturnsOkWhenAddingSuccesful()
        {
            // Arrange
            var customerId = GetRandomInt();
            var itemToAddDto = GetItemToAddDto();
            _basketServiceMock.Setup(x => x.AddItemToBasket(customerId, itemToAddDto)).ReturnsAsync(true);

            // Act
            var result = await _sut.AddItemToBasket(customerId, itemToAddDto);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
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

        [Test]
        public async Task RemoveItemFromBasketCallsServiceWithCustomerIdAndItemId()
        {
            // Arrange
            var customerId = GetRandomInt();
            var itemId = GetRandomInt();

            // Act
            await _sut.RemoveItemFromBasket(customerId, itemId);

            // Assert
            _basketServiceMock.Verify(x => x.RemoveItemFromBasktet(customerId, itemId), Times.Once);
        }

        [Test]
        public async Task RemoveItemFromBasketReturnsOkWhenRemovingSuccessful()
        {
            // Arrange
            var customerId = GetRandomInt();
            var itemId = GetRandomInt();
            _basketServiceMock.Setup(x => x.RemoveItemFromBasktet(customerId, itemId)).ReturnsAsync(true);

            // Act
            var result = await _sut.RemoveItemFromBasket(customerId, itemId);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task RemoveItemFromBasketReturnsBadRequestIfItemNotInBasket()
        {
            // Arrange
            var customerId = GetRandomInt();
            var itemId = GetRandomInt();
            _basketServiceMock.Setup(x => x.RemoveItemFromBasktet(customerId, itemId)).ThrowsAsync(new ItemNotInTheBasketException());

            // Act
            var result = await _sut.RemoveItemFromBasket(customerId, itemId);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task RemoveItemFromBasketReturnsBadRequestIfServiceReturnsFalse()
        {
            // Arrange
            var customerId = GetRandomInt();
            var itemId = GetRandomInt();
            _basketServiceMock.Setup(x => x.RemoveItemFromBasktet(customerId, itemId)).ReturnsAsync(false);

            // Act
            var result = await _sut.RemoveItemFromBasket(customerId, itemId);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task UpdateItemInTheBasketReturnsBadRequestIfItemIsNull()
        {
            // Arrange
            var customerId = GetRandomInt();

            // Act
            var result = await _sut.UpdateItemInTheBasket(customerId, null);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task UpdateItemInTheBasketReturnsBadRequestIfItemIsInvalid()
        {
            // Arrange
            var customerId = GetRandomInt();
            var itemToUpdateDto = GetItemToUpdateDto(quantity: -1);

            // Act
            var result = await _sut.UpdateItemInTheBasket(customerId, itemToUpdateDto);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task UpdateItemInTheBasketReturnsNoContentIfUpdateSuccessful()
        {
            // Arrange
            var customerId = GetRandomInt();
            var itemToUpdateDto = GetItemToUpdateDto();
            _basketServiceMock.Setup(x => x.UpdateBasketItem(customerId, itemToUpdateDto)).ReturnsAsync(true);

            // Act
            var result = await _sut.UpdateItemInTheBasket(customerId, itemToUpdateDto);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task UpdateItemInTheBasketReturnsBadRequestIfUpdateFails()
        {
            // Arrange
            var customerId = GetRandomInt();
            var itemToUpdateDto = GetItemToUpdateDto();
            _basketServiceMock.Setup(x => x.UpdateBasketItem(customerId, itemToUpdateDto)).ReturnsAsync(false);

            // Act
            var result = await _sut.UpdateItemInTheBasket(customerId, itemToUpdateDto);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task ClearBasketCallsServiceWithCustomerId()
        {
            // Arrange
            var customerId = GetRandomInt();

            // Act
            await _sut.ClearBasket(customerId);

            // Assert
            _basketServiceMock.Verify(x => x.ClearBasket(customerId), Times.Once);
        }

        [Test]
        public async Task ClearBasketReturnsOkIfClearingSuccessful()
        {
            // Arrange
            var customerId = GetRandomInt();
            _basketServiceMock.Setup(x => x.ClearBasket(customerId)).ReturnsAsync(true);

            // Act
            var result = await _sut.ClearBasket(customerId);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task ClearBasketReturnsBadRequestIfClearingFails()
        {
            // Arrange
            var customerId = GetRandomInt();
            _basketServiceMock.Setup(x => x.ClearBasket(customerId)).ReturnsAsync(false);

            // Act
            var result = await _sut.ClearBasket(customerId);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }



        private ItemToUpdateDto GetItemToUpdateDto(int? quantity = null)
        {
            return new ItemToUpdateDto
            {
                ItemId = GetRandomInt(),
                Quantity = quantity ?? GetRandomInt()
            };
        }


        private ItemToAddDto GetItemToAddDto(int? quantity = null)
        {
            return new ItemToAddDto
            {
                ItemId = GetRandomInt(),
                Quantity = quantity ?? GetRandomInt()
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
