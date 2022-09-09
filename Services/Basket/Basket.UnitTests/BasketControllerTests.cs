using Basket.Api.Controllers;
using Basket.Api.Data;
using Basket.Api.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Basket.UnitTests {

    public class BasketControllerTests {
        private readonly Mock<IBasketRepo> repositoryStub = new Mock<IBasketRepo>();

        [Fact]
        public async Task GetBasketByIdAsync_IdIsInvalid_ReturnsNotFound() {
            //Arrange
            repositoryStub.Setup(repo => repo.GetBasketByIdAsync("id"))
                            .ReturnsAsync((CustomerBasket?)null);

            var controller = new BasketController(repositoryStub.Object);

            //Act
            var result = await controller.GetBasketByIdAsync("id");

            //Assert
            result.Should().BeOfType<ActionResult<CustomerBasket>>();
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetBasketByIdAsync_IdIsValid_ReturnsOk() {
            //Arrange
            var basket = GetCustomerBasket();

            repositoryStub.Setup(repo => repo.GetBasketByIdAsync("id"))
                            .ReturnsAsync(basket);

            var controller = new BasketController(repositoryStub.Object);

            //Act
            var result = await controller.GetBasketByIdAsync("id");

            //Assert
            result.Should().BeOfType<ActionResult<CustomerBasket>>();
            result.Result.Should().BeOfType<OkObjectResult>();
            (result.Result as OkObjectResult)?.Value.Should().BeEquivalentTo(basket);
        }

        [Fact]
        public async Task UpdateBasketAsync_IdIsInvalid_ReturnsNotFound() {
            //Arrange
            var basket = GetCustomerBasket();

            repositoryStub.Setup(repo => repo.UpdateBasketAsync(basket))
                            .ReturnsAsync((CustomerBasket?)null);

            var controller = new BasketController(repositoryStub.Object);

            //Act
            var result = await controller.UpdateBasketAsync(basket);

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task UpdateBasketAsync_IdIsValid_ReturnsCreatedAtAction() {
            //Arrange
            var basket = GetCustomerBasket();

            repositoryStub.Setup(repo => repo.UpdateBasketAsync(basket))
                            .ReturnsAsync(basket);

            var controller = new BasketController(repositoryStub.Object);

            //Act
            var result = await controller.UpdateBasketAsync(basket);

            //Assert
            result.Should().BeOfType<CreatedAtActionResult>();
            (result as CreatedAtActionResult)?.ActionName.Should().Be("GetBasketByIdAsync");
        }

        [Fact]
        public async Task DeleteBasketAsync_IdIsInvalid_ReturnsNotFound() {
            //Arrange
            var basket = GetCustomerBasket();

            repositoryStub.Setup(repo => repo.DeleteBasketAsync("id"))
                            .ReturnsAsync(false);

            var controller = new BasketController(repositoryStub.Object);

            //Act
            var result = await controller.DeleteBasketAsync("id");

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DeleteBasketAsync_IdIsValid_ReturnsNoContent() {
            //Arrange
            var basket = GetCustomerBasket();

            repositoryStub.Setup(repo => repo.DeleteBasketAsync("id"))
                            .ReturnsAsync(true);

            var controller = new BasketController(repositoryStub.Object);

            //Act
            var result = await controller.DeleteBasketAsync("id");

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }

        private CustomerBasket GetCustomerBasket() => new CustomerBasket() {
            BuyerId = "id",
            Items = new List<BasketItem>() {
                new BasketItem(){
                    Id = "id",
                    ProductId = 1,
                    ProductName = "name",
                    Price = 10,
                    Quantity = 5
                }
            }
        };
    }
}