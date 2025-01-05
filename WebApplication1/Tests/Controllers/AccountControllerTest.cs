using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.EntityFrameworkCore;
using WebApplication1.Controllers;
using WebApplication1.Dto.Account;
using WebApplication1.interfaces;
using WebApplication1.Models;
using Xunit;

namespace WebApplication1.Tests.Controllers
{
    public class AccountControllerTest
    {
        private readonly AccountController _accountController;
        private readonly Mock<UserManager<AppUser>> _mockUserManager;
        private readonly Mock<SignInManager<AppUser>> _mockSignInManager;
        private readonly Mock<ITokenService> _mockTokenService;

        public AccountControllerTest()
        {
            var userStoreMock = new Mock<IUserStore<AppUser>>();
            _mockUserManager = new Mock<UserManager<AppUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            var contextAccessorMock = new Mock<IHttpContextAccessor>();
            var userClaimsPrincipalFactoryMock = new Mock<IUserClaimsPrincipalFactory<AppUser>>();
            _mockSignInManager = new Mock<SignInManager<AppUser>>(
                _mockUserManager.Object,
                contextAccessorMock.Object,
                userClaimsPrincipalFactoryMock.Object,
                null,
                null,
                null,
                null
            );

            _mockTokenService = new Mock<ITokenService>();

            _accountController = new AccountController(_mockUserManager.Object, _mockTokenService.Object, _mockSignInManager.Object);
        }

        [Fact]
        public async Task Login_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            _accountController.ModelState.AddModelError("Username", "Username is required");

            // Act
            var result = await _accountController.login(new LoginDto());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Login_UserNotFound_ReturnsUnauthorized()
        {
            // Arrange
            _mockUserManager.Setup(x => x.Users).Returns(new List<AppUser>().AsQueryable());

            // Act
            var result = await _accountController.login(new LoginDto());

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task Login_InvalidPassword_ReturnsUnauthorized()
        {
            // Arrange
            _mockUserManager.Setup(x => x.Users).Returns(new List<AppUser>().AsQueryable());
            _mockSignInManager.Setup(x => x.CheckPasswordSignInAsync(It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            // Act
            var result = await _accountController.login(new LoginDto());

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        // [Fact]
        // public async Task Login_ValidModel_ReturnsOk()
        // {
        //     // Arrange
        //     var loginDto = new LoginDto { Username = "testuser", Password = "password" };

        //     // Act
        //     var result = await _accountController.login(loginDto);

        //     // Assert
        //     var okResult = Assert.IsType<OkObjectResult>(result);
        //     var returnValue = Assert.IsType<AppUser>(okResult.Value);
        //     Assert.Equal(loginDto.Username, returnValue.UserName);
        // }

        // [Fact]
        // public async Task Login_ValidCredentials_ReturnsOk()
        // {
        //     // Arrange
        //     var loginDto = new LoginDto { Username = "testuser", Password = "password" };
        //     var user = new AppUser { UserName = loginDto.Username, Email = "testuser@example.com" };
        //     _mockUserManager.Setup(x => x.Users).Returns(new List<AppUser> { user }.AsQueryable());
        //     _mockSignInManager.Setup(x => x.CheckPasswordSignInAsync(user, loginDto.Password, false)).ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
        //     _mockTokenService.Setup(x => x.CreateToken(user)).Returns("token");

        //     // Act
        //     var result = await _accountController.login(loginDto);

        //     // Assert
        //     var okResult = Assert.IsType<OkObjectResult>(result);
        //     var returnValue = Assert.IsType<NewUserDto>(okResult.Value);
        //     Assert.Equal(loginDto.Username, returnValue.Username);
        //     Assert.Equal(user.Email, returnValue.Email);
        //     Assert.Equal("token", returnValue.Token);
        // }

        [Fact]
        public async Task Login_UsernameOrPasswordNull_ReturnsUnauthorized()
        {
            // Arrange
            var loginDto = new LoginDto { Username = null, Password = null };

            // Act
            var result = await _accountController.login(loginDto);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Username and password are required", unauthorizedResult.Value);
        }

        [Fact]
        public async Task Register_CreateUserFails_ReturnsStatusCode500()
        {
            // Arrange
            var registerDto = new RegisterDto { Username = "testuser", Email = "testuser@example.com", Password = "password" };
            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Create user failed" }));

            // Act
            var result = await _accountController.register(registerDto);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task Register_AddToRoleFails_ReturnsStatusCode500()
        {
            // Arrange
            var registerDto = new RegisterDto { Username = "testuser", Email = "testuser@example.com", Password = "password" };
            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            _mockUserManager.Setup(x => x.AddToRoleAsync(It.IsAny<AppUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Add to role failed" }));

            // Act
            var result = await _accountController.register(registerDto);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task Register_ValidModel_ReturnsOk()
        {
            // Arrange
            var registerDto = new RegisterDto { Username = "testuser", Email = "testuser@example.com", Password = "password" };
            var appUser = new AppUser { UserName = registerDto.Username, Email = registerDto.Email };
            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            _mockUserManager.Setup(x => x.AddToRoleAsync(It.IsAny<AppUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            _mockTokenService.Setup(x => x.CreateToken(It.IsAny<AppUser>())).Returns("token");

            // Act
            var result = await _accountController.register(registerDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<NewUserDto>(okResult.Value);
            Assert.Equal(registerDto.Username, returnValue.Username);
            Assert.Equal(registerDto.Email, returnValue.Email);
            Assert.Equal("token", returnValue.Token);
        }

        [Fact]
        public async Task Register_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            _accountController.ModelState.AddModelError("Username", "Username is required");

            // Act
            var result = await _accountController.register(new RegisterDto());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}


