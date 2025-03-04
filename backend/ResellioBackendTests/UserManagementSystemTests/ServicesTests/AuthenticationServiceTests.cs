using Microsoft.Extensions.Configuration;
using Moq;
using ResellioBackend.UserManagementSystem.DTOs;
using ResellioBackend.UserManagementSystem.Results;
using ResellioBackend.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResellioBackend.UserManagementSystem.Services.Abstractions;
using ResellioBackend.UserManagementSystem.Services.Implementations;
using ResellioBackend.UserManagementSystem.Repositories.Abstractions;
using ResellioBackend.UserManagementSystem.Models.Base;

namespace ResellioBackendTests.UserManagementSystemTests.ServicesTests
{
    public class AuthenticationServiceTests
    {
        private readonly Mock<IUsersRepository<UserBase>> _userRepositoryMock;
        private readonly Mock<IPasswordService> _passwordServiceMock;
        private readonly Mock<ITokenGenerator> _tokenGeneratorMock;
        private readonly Mock<IConfiguration> _configurationMock;

        private readonly AuthenticationService _authenticationService;

        public AuthenticationServiceTests()
        {
            _userRepositoryMock = new Mock<IUsersRepository<UserBase>>();
            _passwordServiceMock = new Mock<IPasswordService>();
            _tokenGeneratorMock = new Mock<ITokenGenerator>();
            _configurationMock = new Mock<IConfiguration>();

            _authenticationService = new AuthenticationService(
                _userRepositoryMock.Object,
                _passwordServiceMock.Object,
                _tokenGeneratorMock.Object,
                _configurationMock.Object
            );
        }

        [Fact]
        public async Task AuthenticationService_LoginAsync_ShouldReturnError_WhenUserNotFound()
        {
            // Arrange
            var credentials = new LoginCredentialsDto { Email = "test@example.com", Password = "password" };
            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((UserBase)null);

            // Act
            var result = await _authenticationService.LoginAsync(credentials);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Wrong email or password was provided", result.Message);
        }

        [Fact]
        public async Task AuthenticationService_LoginAsync_ShouldReturnError_WhenPasswordIsWrong()
        {
            // Arrange
            var credentials = new LoginCredentialsDto { Email = "test@example.com", Password = "wrongpassword" };
            var user = new Mock<UserBase>();
            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(user.Object);
            _passwordServiceMock.Setup(service => service.VerifyPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            // Act
            var result = await _authenticationService.LoginAsync(credentials);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Wrong email or password was provided", result.Message);

        }

        [Fact]
        public async Task AuthenticationService_LoginAsync_ShouldReturnError_WhenAccountIsNotValid()
        {
            // Arrange
            var credentials = new LoginCredentialsDto { Email = "test@example.com", Password = "wrongpassword" };
            var user = new Mock<UserBase>();
            user.Setup(u => u.ValidateAccount()).Returns(new ResultBase { Success = false, Message = "Whatever" });
            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(user.Object);
            _passwordServiceMock.Setup(service => service.VerifyPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            // Act
            var result = await _authenticationService.LoginAsync(credentials);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task AuthenticationService_LoginAsync_ShouldReturnSuccess_WithValidCredentials()
        {
            // Arrange
            var credentials = new LoginCredentialsDto { Email = "test@example.com", Password = "password" };
            var user = new Mock<UserBase>();
            user.Setup(u => u.ValidateAccount()).Returns(new ResultBase { Success = true });
            user.Setup(u => u.GetClaims()).Returns(new List<System.Security.Claims.Claim>());

            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(user.Object);
            _passwordServiceMock.Setup(service => service.VerifyPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);

            _configurationMock.Setup(config => config["JwtParameters:AuthenticationSecretKey"]).Returns("secretkey");
            _configurationMock.Setup(config => config["JwtParameters:Issuer"]).Returns("issuer");
            _configurationMock.Setup(config => config["JwtParameters:AuthenticationAudience"]).Returns("audience");

            // Act
            _tokenGeneratorMock.Setup(generator => generator.GenerateToken(It.IsAny<List<System.Security.Claims.Claim>>(),
                It.IsAny<int>(), It.IsAny<byte[]>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("generated_token");

            // Assert
            var result = await _authenticationService.LoginAsync(credentials);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("generated_token", result.Token);
        }
    }
}
