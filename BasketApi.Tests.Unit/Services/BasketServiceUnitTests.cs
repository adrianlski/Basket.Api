using AutoMapper;
using BasketApi.Models;
using BasketApi.Repositories;
using BasketApi.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
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
            _repositoryMock.Verify(x => x.GetMany(customerId), Times.Once);
        }
    }
}
