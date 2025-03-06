using Moq;
using ResellioBackend.UserManagementSystem.Models.Base;
using ResellioBackend.UserManagementSystem.Models.Users;
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
    public class RequestEmailVerificationServiceTests
    {
        private readonly Mock<IEmailVerificationService> _mockEmailVerificationService;
        private readonly Mock<IUsersRepository<UserBase>> _mockUsersRepository;
        private readonly RequestEmailVerificationService _service;

        public RequestEmailVerificationServiceTests()
        {
            _mockEmailVerificationService = new Mock<IEmailVerificationService>();
            _mockUsersRepository = new Mock<IUsersRepository<UserBase>>();
            _service = new RequestEmailVerificationService(_mockEmailVerificationService.Object, _mockUsersRepository.Object);
        }

        [Fact]
        public async Task RequestEmailVerificationService_ResentEmailVerificationMessageAsync_UserNotFound_ReturnsFailure()
        {
            // Arrange
            _mockUsersRepository.Setup(repo => repo.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((UserBase)null);

            // Act
            var result = await _service.ResentEmailVerificationMessageAsync("test@example.com");

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task RequestEmailVerificationService_ResentEmailVerificationMessageAsync_UserActive_ReturnsFailure()
        {
            // Arrange
            var inactiveUser = new Customer { IsActive = true };
            _mockUsersRepository.Setup(repo => repo.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(inactiveUser);

            // Act
            var result = await _service.ResentEmailVerificationMessageAsync("test@example.com");

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task RequestEmailVerificationService_ResentEmailVerificationMessageAsync_ValidUser_SendsVerificationEmailAndReturnsSuccess()
        {
            // Arrange
            var activeUser = new Customer { IsActive = false };
            _mockUsersRepository.Setup(repo => repo.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(activeUser);

            // Act
            var result = await _service.ResentEmailVerificationMessageAsync("test@example.com");

            // Assert
            _mockEmailVerificationService.Verify(service => service.CreateAndSendVerificationEmailAsync(activeUser), Times.Once);
            Assert.True(result.Success);
        }
    }
}
