using System;
using AutoMapper;
using BasketApi.Dtos;
using BasketApi.Models;
using BasketApi.Repositories;
using BasketApi.Services;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BasketApi.Tests.Unit.Services
{
    [TestFixture]
    public class BasketServiceUnitTests : TestBase
    {
        private BasketService _sut;
        private Mock<IRepository<BasketItem>> _repositoryMock;
        private Mock<IMapper> _mapperMock;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IRepository<BasketItem>>();
            _mapperMock = new Mock<IMapper>();
            _sut = new BasketService(_repositoryMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task GetBasketCallsBasketRepository()
        {
            // Arrange
            var customerId = GetRandomInt();

            // Act
            await _sut.GetBasket(customerId);

            // Assert
            _repositoryMock.Verify(x => x.GetManyAsync(y => y.CustomerId == customerId), Times.Once);
        }

        [Test]
        public async Task GetBasketCallsMapperWithItems()
        {
            // Arrange
            var customerId = GetRandomInt();
            var items = GetBasketItems();
            _repositoryMock.Setup(x => x.GetManyAsync(y => y.CustomerId == customerId)).ReturnsAsync(items);

            // Act
            await _sut.GetBasket(customerId);

            // Assert
            _mapperMock.Verify(x => x.Map<BasketToReturnDto>(items), Times.Once);
        }

        [Test]
        public async Task GetBasketReturnsBasketToReturnDto()
        {
            // Arrange
            var customerId = GetRandomInt();
            var items = GetBasketItems();
            var itemsDto = GetBasketItemsDto();
            _repositoryMock.Setup(x => x.GetManyAsync(It.IsAny<Expression<Func<BasketItem, bool>>>())).ReturnsAsync(items);
            _mapperMock.Setup(x => x.Map<BasketToReturnDto>(items)).Returns(itemsDto);

            // Act
            var result = await _sut.GetBasket(customerId);

            // Assert
            Assert.IsInstanceOf<BasketToReturnDto>(result);
            Assert.AreEqual(itemsDto.CustomerId, result.CustomerId);
        }

        [Test]
        public async Task AddItemThrowsExceptionIfItemExistsInRepo()
        {
            // Arrange
            var customerId = GetRandomInt();
            var itemToAdd = GetItemToAdd();
            var itemId = GetRandomInt();
            //_repositoryMock.Setup(x => x.ExistsAsync(It.IsAny<Expresion>()))

            // Act
            await _sut.AddItemToBasket(customerId, itemToAdd);

            // Assert
            _repositoryMock.Verify(x => x.GetOneAsync(y => y.CustomerId == customerId && y.ItemId == itemId), Times.Once);
        }

        private ItemToAddDto GetItemToAdd()
        {
            return new ItemToAddDto { ItemId = GetRandomInt()};
        }

        private BasketToReturnDto GetBasketItemsDto()
        {
            return new BasketToReturnDto { CustomerId = GetRandomInt(), Items = new List<ItemToReturnDto>()};
        }

        private List<BasketItem> GetBasketItems()
        {
            return new List<BasketItem>
            {
                new BasketItem()
            };
        }
    }
}
