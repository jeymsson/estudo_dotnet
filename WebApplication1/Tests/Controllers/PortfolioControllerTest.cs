using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApplication1.Controllers;
using WebApplication1.Dto.Portfolio;
using WebApplication1.interfaces;
using WebApplication1.Models;
using Xunit;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Collections.Generic;

namespace WebApplication1.Tests.Controllers
{
    public class PortfolioControllerTest
    {
        private readonly Mock<UserManager<AppUser>> _userManagerMock;
        private readonly Mock<IPortfolioRepository> _portfolioRepoMock;
        private readonly Mock<IStockRepository> _stockRepoMock;
        private readonly PortfolioController _controller;

        public PortfolioControllerTest()
        {
            _userManagerMock = new Mock<UserManager<AppUser>>(Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
            _portfolioRepoMock = new Mock<IPortfolioRepository>();
            _stockRepoMock = new Mock<IStockRepository>();
            _controller = new PortfolioController(_userManagerMock.Object, _portfolioRepoMock.Object, _stockRepoMock.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "testuser")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Fact]
        public async Task getUserPortfolio_ReturnsOkResult_WithUserPortfolio()
        {
            // Arrange
            var appUser = new AppUser { UserName = "testuser" };
            var stocks = new List<Stock> { new Stock { Id = 1, Symbol = "AAPL" } };

            _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(appUser);
            _portfolioRepoMock.Setup(x => x.getUserPortfolio(It.IsAny<AppUser>())).ReturnsAsync(stocks);

            // Act
            var result = await _controller.getUserPortfolio();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var portfolio = Assert.IsType<List<Stock>>(okResult.Value);
            Assert.Equal(stocks, portfolio);
        }

        [Fact]
        public async Task addStock_ReturnsBadRequest_WhenStockNotFound()
        {
            // Arrange
            var createPortfolioRequestDto = new CreatePortfolioRequestDto { Symbol = "AAPL" };
            var appUser = new AppUser { UserName = "testuser" };

            _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(appUser);
            _stockRepoMock.Setup(x => x.findStockByName(It.IsAny<string>())).ReturnsAsync((Stock) null);

            // Act
            var result = await _controller.addStock(createPortfolioRequestDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Stock not found", badRequestResult.Value);
        }

        [Fact]
        public async Task addStock_ReturnsBadRequest_WhenStockAlreadyInPortfolio()
        {
            // Arrange
            var createPortfolioRequestDto = new CreatePortfolioRequestDto { Symbol = "AAPL" };
            var appUser = new AppUser { UserName = "testuser" };
            var stock = new Stock { Id = 1, Symbol = "AAPL" };
            var portfolio = new Portfolio { AppUserId = appUser.Id, StockId = stock.Id };

            _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(appUser);
            _stockRepoMock.Setup(x => x.findStockByName(It.IsAny<string>())).ReturnsAsync(stock);
            _portfolioRepoMock.Setup(x => x.findPortfolioByStockAsync(stock)).ReturnsAsync(portfolio);

            // Act
            var result = await _controller.addStock(createPortfolioRequestDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Stock already in portfolio", badRequestResult.Value);
        }

        [Fact]
        public async Task addStock_ReturnsOkResult_WithPortfolio()
        {
            // Arrange
            var createPortfolioRequestDto = new CreatePortfolioRequestDto { Symbol = "AAPL" };
            var appUser = new AppUser { UserName = "testuser" };
            var stock = new Stock { Id = 1, Symbol = "AAPL" };

            _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(appUser);
            _stockRepoMock.Setup(x => x.findStockByName(It.IsAny<string>())).ReturnsAsync(stock);
            _portfolioRepoMock.Setup(x => x.findPortfolioByStockAsync(stock)).ReturnsAsync((Portfolio?) null);
            _portfolioRepoMock.Setup(x => x.createAsync(It.IsAny<Portfolio>())).ReturnsAsync((Portfolio p) => p);

            // Act
            var result = await _controller.addStock(createPortfolioRequestDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var portfolio = Assert.IsType<Portfolio>(okResult.Value);
            Assert.Equal(appUser.Id, portfolio.AppUserId);
            Assert.Equal(stock.Id, portfolio.StockId);
        }

        [Fact]
        public async Task deleteStock_ReturnsBadRequest_WhenPortfolioNotFound()
        {
            // Arrange
            var appUser = new AppUser { UserName = "testuser" };

            _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(appUser);
            _portfolioRepoMock.Setup(x => x.findPortfolioByIdAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync((Portfolio) null);

            // Act
            var result = await _controller.deleteStock(1);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Portfolio not found", badRequestResult.Value);
        }

        [Fact]
        public async Task deleteStock_ReturnsUnauthorized_WhenUserIsNotOwner()
        {
            // Arrange
            var appUser = new AppUser { UserName = "testuser", Id = "1" };
            var portfolio = new Portfolio { AppUserId = "2", StockId = 1 };

            _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(appUser);
            _portfolioRepoMock.Setup(x => x.findPortfolioByIdAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(portfolio);

            // Act
            var result = await _controller.deleteStock(1);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Unauthorized, you are not the owner of this portfolio", unauthorizedResult.Value);
        }

        [Fact]
        public async Task deleteStock_ReturnsOkResult_WithDeletedPortfolio()
        {
            // Arrange
            var appUser = new AppUser { UserName = "testuser", Id = "1" };
            var portfolio = new Portfolio { AppUserId = "1", StockId = 1 };

            _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(appUser);
            _portfolioRepoMock.Setup(x => x.findPortfolioByIdAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(portfolio);
            _portfolioRepoMock.Setup(x => x.deletePortfolioAsync(It.IsAny<Portfolio>())).ReturnsAsync((Portfolio p) => p);

            // Act
            var result = await _controller.deleteStock(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(portfolio, okResult.Value);
        }
    }
}