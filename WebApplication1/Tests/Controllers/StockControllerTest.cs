using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApplication1.Controllers;
using WebApplication1.Dto.Stock;
using WebApplication1.Helpers;
using WebApplication1.interfaces;
using WebApplication1.Models;
using Xunit;

namespace WebApplication1.Tests.Controllers
{
    public class StockControllerTest
    {
        private readonly Mock<IStockRepository> _mockRepo;
        private readonly StockController _controller;

        public StockControllerTest()
        {
            _mockRepo = new Mock<IStockRepository>();
            _controller = new StockController(null, _mockRepo.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfStocks()
        {
            // Arrange
            var query = new QueryObject();
            var stocks = new List<Stock>
            {
                new Stock { Id = 1, CompanyName = "Test Stock" }
            };
            _mockRepo.Setup(repo => repo.getAllAsync(query)).ReturnsAsync(stocks);

            // Act
            var result = await _controller.GetAll(query);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Stock>>(okResult.Value);
            Assert.Single(returnValue);
            Assert.Equal(stocks.First().CompanyName, returnValue.First().CompanyName);
        }

        [Fact]
        public async Task GetAll_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Error", "Model state is invalid");
            var query = new QueryObject();

            // Act
            var result = await _controller.GetAll(query);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WithStock()
        {
            // Arrange
            var stockId = 1;
            var stock = new Stock { Id = stockId, CompanyName = "Test Stock" };
            _mockRepo.Setup(repo => repo.FindAsync(stockId)).ReturnsAsync(stock);

            // Act
            var result = await _controller.getById(stockId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<StockDto>(okResult.Value);
            Assert.Equal(stockId, returnValue.Id);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenStockNotFound()
        {
            // Arrange
            var stockId = 1;
            _mockRepo.Setup(repo => repo.FindAsync(stockId)).ReturnsAsync((Stock) null);

            // Act
            var result = await _controller.getById(stockId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetById_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Error", "Model state is invalid");
            var stockId = 1;

            // Act
            var result = await _controller.getById(stockId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtAction_WithStock()
        {
            // Arrange
            var stockDto = new CreateStockRequestDto { CompanyName = "New Stock" };
            var stockModel = new Stock { Id = 1, CompanyName = "New Stock" };
            _mockRepo.Setup(repo => repo.AddAsync(It.IsAny<Stock>())).ReturnsAsync(stockModel);

            // Act
            var result = await _controller.create(stockDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<StockDto>(createdAtActionResult.Value);
            Assert.Equal(stockDto.CompanyName, returnValue.CompanyName);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Error", "Model state is invalid");
            var stockDto = new CreateStockRequestDto { CompanyName = "New Stock" };

            // Act
            var result = await _controller.create(stockDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsOkResult_WithUpdatedStock()
        {
            // Arrange
            var stockId = 1;
            var stockDto = new UpdateStockRequestDto { CompanyName = "Updated Stock" };
            var stockModel = new Stock { Id = stockId, CompanyName = "Updated Stock" };
            _mockRepo.Setup(repo => repo.UpdateAsync(stockId, It.IsAny<Stock>())).ReturnsAsync(stockModel);

            // Act
            var result = await _controller.update(stockId, stockDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<StockDto>(okResult.Value);
            Assert.Equal(stockDto.CompanyName, returnValue.CompanyName);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenStockNotFound()
        {
            // Arrange
            var stockId = 1;
            var stockDto = new UpdateStockRequestDto { CompanyName = "Updated Stock" };
            _mockRepo.Setup(repo => repo.UpdateAsync(stockId, It.IsAny<Stock>())).ReturnsAsync((Stock?) null);

            // Act
            var result = await _controller.update(stockId, stockDto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Error", "Model state is invalid");
            var stockId = 1;
            var stockDto = new UpdateStockRequestDto { CompanyName = "Updated Stock" };

            // Act
            var result = await _controller.update(stockId, stockDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenStockDeleted()
        {
            // Arrange
            var stockId = 1;
            var stock = new Stock { Id = stockId, CompanyName = "Test Stock" };
            _mockRepo.Setup(repo => repo.FindAsync(stockId)).ReturnsAsync(stock);
            _mockRepo.Setup(repo => repo.DeleteAsync(stock)).ReturnsAsync(stock);

            // Act
            var result = await _controller.delete(stockId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenStockNotFound()
        {
            // Arrange
            var stockId = 1;
            _mockRepo.Setup(repo => repo.FindAsync(stockId)).ReturnsAsync((Stock) null);

            // Act
            var result = await _controller.delete(stockId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Error", "Model state is invalid");
            var stockId = 1;

            // Act
            var result = await _controller.delete(stockId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
