using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using WebApplication1.Controllers;
using WebApplication1.Data;
using WebApplication1.Dto.Comment;
using WebApplication1.interfaces;
using WebApplication1.Models;
using Xunit;

namespace WebApplication1.Tests.Controllers
{
    public class CommentControllerTest
    {
        private readonly Mock<ICommentRepository> _mockCommentRepository;
        private readonly Mock<IStockRepository> _mockStockRepository;
        private readonly CommentController _controller;
        private readonly ApplicationDbContext _context;

        public CommentControllerTest()
        {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        _context = new ApplicationDbContext(options);

        _mockCommentRepository = new Mock<ICommentRepository>();
        _mockStockRepository = new Mock<IStockRepository>();

        _controller = new CommentController(_context, _mockCommentRepository.Object, _mockStockRepository.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfComments()
        {
            // Arrange
            var comments = new List<Comment> { new Comment { Id = 1, Content = "Test Comment" } };
            _mockCommentRepository.Setup(repo => repo.getAllAsync()).ReturnsAsync(comments);

            // Act
            var result = await _controller.getAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Comment>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetAll_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Error", "Model state is invalid");

            // Act
            var result = await _controller.getAll();

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WithComment()
        {
            // Arrange
            var comment = new Comment { Id = 1, Content = "Test Comment" };
            _mockCommentRepository.Setup(repo => repo.FindAsync(1)).ReturnsAsync(comment);

            // Act
            var result = await _controller.getById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<CommentDto>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenCommentDoesNotExist()
        {
            // Arrange
            _mockCommentRepository.Setup(repo => repo.FindAsync(1)).ReturnsAsync((Comment) null);

            // Act
            var result = await _controller.getById(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetById_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Error", "Model state is invalid");

            // Act
            var result = await _controller.getById(1);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsOkResult_WithCreatedComment()
        {
            // Arrange
            var commentDto = new CreateCommentRequestDto { Content = "New Comment" };
            var comment = new Comment { Id = 1, Content = "New Comment" };
            _mockStockRepository.Setup(repo => repo.stockExists(1)).ReturnsAsync(true);
            _mockCommentRepository.Setup(repo => repo.AddAsync(It.IsAny<Comment>())).ReturnsAsync(comment);

            // Act
            var result = await _controller.create(1, commentDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Comment>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Error", "Model state is invalid");
            var commentDto = new CreateCommentRequestDto { Content = "New Comment" };

            // Act
            var result = await _controller.create(1, commentDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenStockDoesNotExist()
        {
            // Arrange
            var commentDto = new CreateCommentRequestDto { Content = "New Comment" };
            _mockStockRepository.Setup(repo => repo.stockExists(1)).ReturnsAsync(false);

            // Act
            var result = await _controller.create(1, commentDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Stock not exists", badRequestResult.Value);
        }


        [Fact]
        public async Task Update_ReturnsOkResult_WithUpdatedComment()
        {
            // Arrange
            var updateDto = new UpdateCommentRequestDto { Content = "Updated Comment" };
            var comment = new Comment { Id = 1, Content = "Updated Comment" };
            _mockStockRepository.Setup(repo => repo.stockExists(1)).ReturnsAsync(true);
            _mockCommentRepository.Setup(repo => repo.UpdateAsync(1, It.IsAny<Comment>())).ReturnsAsync(comment);

            // Act
            var result = await _controller.update(1, updateDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<CommentDto>(okResult.Value);
            Assert.Equal("Updated Comment", returnValue.Content);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Error", "Model state is invalid");
            var updateDto = new UpdateCommentRequestDto { Content = "Updated Comment" };

            // Act
            var result = await _controller.update(1, updateDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenStockDoesNotExist()
        {
            // Arrange
            var updateDto = new UpdateCommentRequestDto { Content = "Updated Comment" };
            _mockStockRepository.Setup(repo => repo.stockExists(1)).ReturnsAsync(false);

            // Act
            var result = await _controller.update(1, updateDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Stock not exists", badRequestResult.Value);
        }


        [Fact]
        public async Task Delete_ReturnsOkResult_WithDeletedComment()
        {
            // Arrange
            var comment = new Comment { Id = 1, Content = "Test Comment" };
            _mockCommentRepository.Setup(repo => repo.FindAsync(1)).ReturnsAsync(comment);
            _mockCommentRepository.Setup(repo => repo.DeleteAsync(comment)).ReturnsAsync(comment);

            // Act
            var result = await _controller.delete(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<CommentDto>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenCommentDoesNotExist()
        {
            // Arrange
            _mockCommentRepository.Setup(repo => repo.FindAsync(1)).ReturnsAsync((Comment) null);

            // Act
            var result = await _controller.delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Error", "Model state is invalid");

            // Act
            var result = await _controller.delete(1);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

    }
}