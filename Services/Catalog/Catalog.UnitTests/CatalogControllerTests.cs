using Catalog.Api.Controllers;
using Catalog.Api.Data;
using Catalog.Api.Dtos;
using Catalog.Api.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Catalog.UnitTests {

    public class CatalogControllerTests {
        
        private readonly Mock<ICatalogRepo> repositoryStub = new Mock<ICatalogRepo>();

        [Fact]
        public async Task CreateItemAsync_ItemIsValid_ReturnsCreatedAtAction() {
            //Arrange
            CatalogItem item = GetCatalogItem();

            var controller = new CatalogController(repositoryStub.Object);

            //Act
            var result = await controller.CreateItemAsync(item);

            //Assert
            result.Should().BeOfType<CreatedAtActionResult>();
        }

        [Fact]
        public void GetAllItems_RequestIsValid_ReturnsOk() {
            //Arrange
            var controller = new CatalogController(repositoryStub.Object);

            //Act
            var result = controller.GetAllItems();

            //Assert
            result.Should().BeOfType<ActionResult<IEnumerable<CatalogItem>>>();
            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void GetItemById_IdIsLessThanZero_ReturnsBadRequest() {
            //Arrange
            var controller = new CatalogController(repositoryStub.Object);

            //Act
            var result = controller.GetItemById(-1);

            //Assert
            result.Should().BeOfType<ActionResult<CatalogItem>>();
            result.Result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public void GetItemById_IdIsInvalid_ReturnsNotFound() {
            //Arrange
            repositoryStub.Setup(repo => repo.GetItemById(10))
                            .Returns((CatalogItem?)null);

            var controller = new CatalogController(repositoryStub.Object);

            //Act
            var result = controller.GetItemById(10);

            //Assert
            result.Should().BeOfType<ActionResult<CatalogItem>>();
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void GetItemById_IdIsValid_ReturnsOk() {
            //Arrange
            CatalogItem item = GetCatalogItem();

            repositoryStub.Setup(repo => repo.GetItemById(10))
                            .Returns(item);

            var controller = new CatalogController(repositoryStub.Object);

            //Act
            var result = controller.GetItemById(10);

            //Assert
            result.Should().BeOfType<ActionResult<CatalogItem>>();
            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task UpdateItemAsync_UpdateItemDtoIsInvalid_ReturnsNotFound() {
            //Arrange
            UpdateItemDto item = GetUpdateItemDto();

            repositoryStub.Setup(repo => repo.UpdateItemAsync(item))
                            .ReturnsAsync((CatalogItem?)null);

            var controller = new CatalogController(repositoryStub.Object);

            //Act
            var result = await controller.UpdateItemAsync(item);

            //Assert
            result.Should().BeOfType<NotFoundResult>();        
        }

        [Fact]
        public async Task UpdateItemAsync_UpdateItemDtoIsValid_ReturnsCreatedAtAction() {
            //Arrange
            UpdateItemDto item = GetUpdateItemDto();
            CatalogItem catalogItem = GetCatalogItem();

            repositoryStub.Setup(repo => repo.UpdateItemAsync(item))
                            .ReturnsAsync(catalogItem);

            var controller = new CatalogController(repositoryStub.Object);

            //Act
            var result = await controller.UpdateItemAsync(item);

            //Assert
            result.Should().BeOfType<CreatedAtActionResult>();        
        }
        
        [Fact]
        public async Task DeleteItemAsync_IdIsInvalid_ReturnsBadRequest() {
            //Arrange
            repositoryStub.Setup(repo => repo.GetItemById(10))
                            .Returns((CatalogItem?)null);

            var controller = new CatalogController(repositoryStub.Object);

            //Act
            var result = await controller.DeleteItemAsync(10);

            //Assert
            result.Should().BeOfType<BadRequestResult>();        
        }

        [Fact]
        public async Task DeleteItemAsync_IdIsValid_ReturnsNoContent() {
            //Arrange
            CatalogItem item = GetCatalogItem();

            repositoryStub.Setup(repo => repo.GetItemById(10))
                            .Returns(item);

            var controller = new CatalogController(repositoryStub.Object);

            //Act
            var result = await controller.DeleteItemAsync(10);

            //Assert
            result.Should().BeOfType<NoContentResult>();        
        }

        private CatalogItem GetCatalogItem() => new CatalogItem() {
                Id = 1,
                Name = "TestName",
                Description = "TestDescription",
                Price = 10,
                Brand = "TestBrand",
                AvailableStock = 5 };

        private UpdateItemDto GetUpdateItemDto() => new UpdateItemDto() {
                Id = 1,
                Name = "TestName",
                Description = "TestDescription",
                Price = 10,
                Brand = "TestBrand",
                AvailableStock = 5 };        
    }
}