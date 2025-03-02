using Moq;
using ResellioBackend.DTOs.Base;
using ResellioBackend.DTOs.Users;
using ResellioBackend.Factories.Abstractions;
using ResellioBackend.Models.Base;
using ResellioBackend.Models.Users;
using ResellioBackend.Repositories.Abstractions;
using ResellioBackend.Services.Abstractions;
using ResellioBackend.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResellioBackendTests.ServicesTests
{
    public class RegistrationServiceTests
    {
        private readonly Mock<IUsersRepository<UserBase>> _userRepositoryMock;
        private readonly Mock<IUserFactory> _userFactoryMock;
        private readonly Mock<IPasswordService> _passwordServiceMock;
        private readonly RegistrationService _registrationService;

        public RegistrationServiceTests()
        {
            _userRepositoryMock = new Mock<IUsersRepository<UserBase>>();
            _userFactoryMock = new Mock<IUserFactory>();
            _passwordServiceMock = new Mock<IPasswordService>();
            _registrationService = new RegistrationService(_userRepositoryMock.Object, _userFactoryMock.Object, _passwordServiceMock.Object);
        }

        [Fact]
        public async Task RegistrationService_RegisterUserAsync_ShouldReturnError_WhenUserWithEmailAlreadyExists()
        {
            // Arrange
            var dto = new RegisterCustomerDto { Email = "existing@example.com", Password = "password123" };
            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(dto.Email)).ReturnsAsync(new Customer());

            // Act
            var result = await _registrationService.RegisterUserAsync(dto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("User with such email already exists", result.Message);
            _userRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<UserBase>()), Times.Never);
        }

        [Fact]
        public async Task RegistrationService_RegisterUserAsync_ShouldAddUserAndReturnSuccess_WhenEmailIsUnique()
        {
            // Arrange
            var dto = new RegisterCustomerDto { Email = "new@example.com", Password = "password123" };
            var newUser = new Customer { UserId = 123 };

            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(dto.Email)).ReturnsAsync((UserBase)null);
            _userFactoryMock.Setup(factory => factory.CreateNewUserWithoutPassword(dto)).Returns(newUser);
            _passwordServiceMock.Setup(service => service.HashPassword(dto.Password)).Returns(("hashedPassword", "salt"));

            // Act
            var result = await _registrationService.RegisterUserAsync(dto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Created successfully", result.Message);
            Assert.Equal(123, result.Id);
            _userRepositoryMock.Verify(repo => repo.AddAsync(newUser), Times.Once);
            Assert.Equal("hashedPassword", newUser.PasswordHash);
            Assert.Equal("salt", newUser.Salt);
        }
    }
}
