using EasyPayChallenge.Application.Handlers;
using EasyPayChallenge.Application.Repositories;
using EasyPayChallenge.Domain;
using EasyPayChallenge.Domain.Models.Commands;
using Microsoft.Extensions.Logging;
using Moq;
using StackExchange.Redis;

namespace EasyPayChallenge.UnitTests
{
    public class CreateProductHandlerTests
    {
        [Fact]
        public async Task Handle_ValidCommand_ReturnsProductDto()
        {
            // Arrange
            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(repo => repo.Create(It.IsAny<Product>())).ReturnsAsync(1);

            var databaseMock = new Mock<IConnectionMultiplexer>();
            databaseMock
                .Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
                .Returns(new Mock<IDatabase>().Object);

            var loggerMock = new Mock<ILogger<CreateProductHandler>>();

            var command = new CreateProductCommand
            {
                Name = "Test Product",
                Brand = "Test Brand",
                PriceAmount = 10.5m,
                Currency = "USD"
            };
            
            var product = new Product
            {
                Name = command.Name,
                Brand = command.Brand,
                Price = new Money(command.PriceAmount, Currency.USD)
            };

            var handler = new CreateProductHandler(productRepositoryMock.Object, databaseMock.Object, loggerMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(command.Name, result.Name);
            Assert.Equal(command.Brand, result.Brand);
            Assert.Equal(product.Price.ToString(), result.Price);
        }

        [Fact]
        public async Task Handle_InvalidCurrency_ThrowsArgumentException()
        {
            // Arrange
            var productRepositoryMock = new Mock<IProductRepository>();
            var databaseMock = new Mock<IConnectionMultiplexer>();
            databaseMock
                .Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
                .Returns(new Mock<IDatabase>().Object);
            var loggerMock = new Mock<ILogger<CreateProductHandler>>();

            var command = new CreateProductCommand
            {
                Name = "Test Product",
                Brand = "Test Brand",
                PriceAmount = 10.5m,
                Currency = "INVALID_CURRENCY"
            };

            var handler = new CreateProductHandler(productRepositoryMock.Object, databaseMock.Object, loggerMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}
