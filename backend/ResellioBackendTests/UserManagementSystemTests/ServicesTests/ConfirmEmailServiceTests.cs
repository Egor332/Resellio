using Moq;
using ResellioBackend.UserManagementSystem.Models.Base;
using ResellioBackend.UserManagementSystem.Models.Users;
using ResellioBackend.UserManagementSystem.Repositories.Abstractions;
using ResellioBackend.UserManagementSystem.Results;
using ResellioBackend.UserManagementSystem.Services.Abstractions;
using ResellioBackend.UserManagementSystem.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResellioBackendTests.UserManagementSystemTests.ServicesTests
{
    public class ConfirmEmailServiceTests
    {
        private readonly Mock<IEmailVerificationTokenService> _tokenServiceMock;
        private readonly Mock<IUsersRepository<UserBase>> _usersRepositoryMock;
        private readonly ConfirmEmailService _confirmEmailService;

        public ConfirmEmailServiceTests()
        {
            _tokenServiceMock = new Mock<IEmailVerificationTokenService>();
            _usersRepositoryMock = new Mock<IUsersRepository<UserBase>>();

            _confirmEmailService = new ConfirmEmailService(
                _tokenServiceMock.Object,
                _usersRepositoryMock.Object
            );
        }

        [Fact]
        public async Task ConfirmEmailService_ConfirmEmailAsync_ShouldReturnSuccess_WhenTokenIsValidAndUserExists()
        {
            // Arrange
            var validToken = "validToken";
            var validUserId = 123;
            var user = new Customer { UserId = validUserId, IsActive = false };

            _tokenServiceMock.Setup(t => t.ValidateEmailVerificationToken(validToken))
                .Returns(new ValidateEmailVerificationTokenResult
                {
                    Success = true,
                    UserId = validUserId
                });

            _usersRepositoryMock.Setup(r => r.GetByIdAsync(validUserId))
                .ReturnsAsync(user);

            // Act
            var result = await _confirmEmailService.ConfirmEmailAsync(validToken);

            // Assert
            Assert.True(result.Success);
            Assert.True(user.IsActive);
            _usersRepositoryMock.Verify(r => r.UpdateAsync(user), Times.Once);
        }

        [Fact]
        public async Task ConfirmEmailService_ConfirmEmailAsync_ShouldReturnFailure_WhenTokenIsInvalid()
        {
            // Arrange
            var invalidToken = "invalidToken";

            _tokenServiceMock.Setup(t => t.ValidateEmailVerificationToken(invalidToken))
                .Returns(new ValidateEmailVerificationTokenResult
                {
                    Success = false,
                    Message = "Invalid token"
                });

            // Act
            var result = await _confirmEmailService.ConfirmEmailAsync(invalidToken);

            // Assert
            Assert.False(result.Success);
            _usersRepositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Never);
            _usersRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<UserBase>()), Times.Never);
        }

        [Fact]
        public async Task ConfirmEmailService_ConfirmEmailAsync_ShouldReturnFailure_WhenUserDoesNotExist()
        {
            // Arrange
            var validToken = "validToken";
            var validUserId = 123;

            _tokenServiceMock.Setup(t => t.ValidateEmailVerificationToken(validToken))
                .Returns(new ValidateEmailVerificationTokenResult
                {
                    Success = true,
                    UserId = validUserId
                });

            _usersRepositoryMock.Setup(r => r.GetByIdAsync(validUserId))
                .ReturnsAsync((UserBase)null);

            // Act
            var result = await _confirmEmailService.ConfirmEmailAsync(validToken);

            // Assert
            Assert.False(result.Success);
            _usersRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<UserBase>()), Times.Never);
        }
    }
}
