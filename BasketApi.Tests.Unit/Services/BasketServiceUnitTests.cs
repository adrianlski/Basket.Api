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
using BasketApi.Exceptions;

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
            _repositoryMock.Verify(x => x.GetManyAsync(It.IsAny<Expression<Func<BasketItem, bool>>>()), Times.Once);
        }

        [Test]
        public async Task GetBasketCallsMapperWithItems()
        {
            // Arrange
            var customerId = GetRandomInt();
            var items = GetBasketItems();
            _repositoryMock.Setup(x => x.GetManyAsync(It.IsAny<Expression<Func<BasketItem, bool>>>()))
                .ReturnsAsync(items);

            // Act
            await _sut.GetBasket(customerId);

            // Assert
            _mapperMock.Verify(x => x.Map<BasketToReturnDto>(items), Times.Once);
        }

        [Test]
        public async Task AddItemThrowsExceptionIfItemExistsInRepo()
        {
            // Arrange
            var customerId = GetRandomInt();
            var itemToAdd = GetItemToAdd();

            _repositoryMock.Setup(x => x.ExistsAsync(It.IsAny<Expression<Func<BasketItem, bool>>>()))
                .ReturnsAsync(true);

            // Act && Assert
            Assert.ThrowsAsync<ItemExistsInTheBasketException>(async () =>
                await _sut.AddItemToBasket(customerId, itemToAdd));
        }

        [Test]
        public async Task AddItemAddsItemToRepository()
        {
            // Arrange
            var customerId = GetRandomInt();
            var itemToAdd = GetItemToAdd();
            var basketItem = GetBasketItem();
            _repositoryMock.Setup(x => x.ExistsAsync(It.IsAny<Expression<Func<BasketItem, bool>>>()))
                .ReturnsAsync(false);
            _mapperMock.Setup(x => x.Map<ItemToAddDto, BasketItem>(itemToAdd)).Returns(basketItem);

            // Act
            await _sut.AddItemToBasket(customerId, itemToAdd);

            // Assert
            _repositoryMock.Verify(x => x.Add(basketItem), Times.Once);
            _repositoryMock.Verify(x => x.SaveAllAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateBasketItemThrowsExceptionIfItemNotFound()
        {
            // Arrange
            var customerId = GetRandomInt();
            var itemToUpdate = GetItemToUpdate();
            _repositoryMock.Setup(x => x.GetOneAsync(It.IsAny<Expression<Func<BasketItem, bool>>>()))
                .ReturnsAsync((BasketItem) null);

            // Act && Assert
            Assert.ThrowsAsync<ItemNotInTheBasketException>(async () =>
                await _sut.UpdateBasketItem(customerId, itemToUpdate));
        }

        [Test]
        public async Task UpdateBasketItemCallsMapper()
        {
            // Arrange
            var customerId = GetRandomInt();
            var itemToUpdate = GetItemToUpdate();
            var basketItem = GetBasketItem();
            _repositoryMock.Setup(x => x.GetOneAsync(It.IsAny<Expression<Func<BasketItem, bool>>>()))
                .ReturnsAsync(basketItem);
            _mapperMock.Setup(x => x.Map(It.IsAny<ItemToUpdateDto>(), It.IsAny<BasketItem>())).Returns(basketItem);

            // Act
            await _sut.UpdateBasketItem(customerId, itemToUpdate);

            // Assert
            _mapperMock.Verify(x => x.Map(itemToUpdate, basketItem), Times.Once());
        }

        [Test]
        public async Task UpdateBasketCallsSaveAsync()
        {
            // Arrange
            var customerId = GetRandomInt();
            var itemToUpdate = GetItemToUpdate();
            var basketItem = GetBasketItem();
            _repositoryMock.Setup(x => x.GetOneAsync(It.IsAny<Expression<Func<BasketItem, bool>>>()))
                .ReturnsAsync(basketItem);
            _mapperMock.Setup(x => x.Map(It.IsAny<ItemToUpdateDto>(), It.IsAny<BasketItem>())).Returns(basketItem);

            // Act
            await _sut.UpdateBasketItem(customerId, itemToUpdate);

            // Assert
            _repositoryMock.Verify(x => x.SaveAllAsync(), Times.Once);
        }

        [Test]
        public async Task RemoveItemFromBasketThrowsExceptionIfItemNotFound()
        {
            // Arrange
            var customerId = GetRandomInt();
            var itemId = GetRandomInt();
            _repositoryMock.Setup(x => x.GetOneAsync(It.IsAny<Expression<Func<BasketItem, bool>>>()))
                .ReturnsAsync((BasketItem) null);

            // Act && Assert
            Assert.ThrowsAsync<ItemNotInTheBasketException>(async () =>
                await _sut.RemoveItemFromBasktet(customerId, itemId));
        }

        [Test]
        public async Task RemoveItemFromBasketDeletesItemFromRepository()
        {
            // Arrange
            var customerId = GetRandomInt();
            var itemId = GetRandomInt();
            var basketItem = GetBasketItem();
            _repositoryMock.Setup(x => x.GetOneAsync(It.IsAny<Expression<Func<BasketItem, bool>>>()))
                .ReturnsAsync(basketItem);

            // Act
            await _sut.RemoveItemFromBasktet(customerId, itemId);

            // Assert
            _repositoryMock.Verify(x => x.DeleteOne(basketItem), Times.Once);
            _repositoryMock.Verify(x => x.SaveAllAsync(), Times.Once);
        }

        [Test]
        public async Task ClearBasketReturnsTrueIfNoItemsToClear()
        {
            // Arrange
            var customerId = GetRandomInt();
            _repositoryMock.Setup(x => x.GetManyAsync(It.IsAny<Expression<Func<BasketItem, bool>>>()))
                .ReturnsAsync(new List<BasketItem>());

            // Act
            var result = await _sut.ClearBasket(customerId);

            // Assert
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task ClearBasketDeletesRangeFromRepository()
        {
            // Arrange
            var customerId = GetRandomInt();
            var basketItems = GetBasketItems();
            _repositoryMock.Setup(x => x.GetManyAsync(It.IsAny<Expression<Func<BasketItem, bool>>>()))
                .ReturnsAsync(basketItems);

            // Act
            await _sut.ClearBasket(customerId);

            // Assert
            _repositoryMock.Verify(x => x.DeleteRange(basketItems), Times.Once);
        }

        private ItemToUpdateDto GetItemToUpdate()
        {
            return new ItemToUpdateDto();
        }

        private BasketItem GetBasketItem()
        {
            return new BasketItem{Item =  new Item()};
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
