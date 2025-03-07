using Moq;
using ResellioBackend.UserManagementSystem.DTOs.Base;
using ResellioBackend.UserManagementSystem.DTOs.Users;
using ResellioBackend.UserManagementSystem.Models.Users;
using ResellioBackend.UserManagementSystem.Factories.Abstractions;
using ResellioBackend.UserManagementSystem.Models.Base;
using ResellioBackend.UserManagementSystem.Repositories.Abstractions;
using ResellioBackend.UserManagementSystem.Services.Abstractions;
using ResellioBackend.UserManagementSystem.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResellioBackendTests.UserManagementSystemTests.ServicesTests
{
    public class RegistrationServiceTests
    {
        private readonly Mock<IUsersRepository<UserBase>> _userRepositoryMock;
        private readonly Mock<IUserFactory> _userFactoryMock;
        private readonly Mock<IPasswordService> _passwordServiceMock;
        private readonly Mock<IEmailVerificationService> _emailVerificationServiceMock;
        private readonly RegistrationService _registrationService;

        public RegistrationServiceTests()
        {
            _userRepositoryMock = new Mock<IUsersRepository<UserBase>>();
            _userFactoryMock = new Mock<IUserFactory>();
            _passwordServiceMock = new Mock<IPasswordService>();
            _emailVerificationServiceMock = new Mock<IEmailVerificationService>();
            _registrationService = new RegistrationService(_userRepositoryMock.Object, _userFactoryMock.Object, _passwordServiceMock.Object, _emailVerificationServiceMock.Object);
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldReturnError_WhenUserWithEmailAlreadyExists()
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
        public async Task RegisterUserAsync_ShouldAddUserAndReturnSuccess_WhenEmailIsUnique()
        {
            // Arrange
            var dto = new RegisterCustomerDto { Email = "new@example.com", Password = "password123" };
            var newUser = new Customer { UserId = 123 };

            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(dto.Email)).ReturnsAsync((UserBase)null);
            _userFactoryMock.Setup(factory => factory.CreateNewUserWithoutPassword(dto)).Returns(newUser);
            _passwordServiceMock.Setup(service => service.HashPassword(dto.Password)).Returns(("hashedPassword", "salt"));
            _emailVerificationServiceMock.Setup(v => v.CreateAndSendVerificationEmailAsync(It.IsAny<UserBase>())).Returns(Task.CompletedTask);

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
