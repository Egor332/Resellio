using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Moq;
using ResellioBackend.Kafka;
using ResellioBackend.UserManagementSystem.DTOs;
using ResellioBackend.UserManagementSystem.Models.Base;
using ResellioBackend.UserManagementSystem.Models.Emails;
using ResellioBackend.UserManagementSystem.Models.Tokens;
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
    public class ResetPasswordServiceTests
    {
        private readonly Mock<IUsersRepository<UserBase>> _usersRepositoryMock;
        private readonly Mock<IPasswordResetTokenService> _passwordResetTokenServiceMock;
        private readonly Mock<IPasswordResetTokenRepository> _tokenRepositoryMock;
        private readonly Mock<IPasswordService> _passwordServiceMock;
        private readonly Mock<IKafkaProducerService> _kafkaProducerServiceMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<LinkGenerator> _linkGeneratorMock;
        private readonly ResetPasswordService _service;

        public ResetPasswordServiceTests()
        {
            _usersRepositoryMock = new Mock<IUsersRepository<UserBase>>();
            _passwordResetTokenServiceMock = new Mock<IPasswordResetTokenService>();
            _tokenRepositoryMock = new Mock<IPasswordResetTokenRepository>();
            _passwordServiceMock = new Mock<IPasswordService>();
            _kafkaProducerServiceMock = new Mock<IKafkaProducerService>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _linkGeneratorMock = new Mock<LinkGenerator>();

            // Setup a default HttpContext for IHttpContextAccessor
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "https";
            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

            _service = new ResetPasswordService(
                _usersRepositoryMock.Object,
                _passwordResetTokenServiceMock.Object,
                _tokenRepositoryMock.Object,
                _passwordServiceMock.Object,
                _kafkaProducerServiceMock.Object,
                _httpContextAccessorMock.Object,
                _linkGeneratorMock.Object
            );
        }

        [Fact]
        public async Task RequestResetPasswordAsync_UserNotFound_ReturnsFalseResult()
        {
            // Arrange
            string email = "nonexistent@example.com";
            _usersRepositoryMock.Setup(x => x.GetByEmailAsync(email))
                                .ReturnsAsync((UserBase)null);

            // Act
            var result = await _service.RequestResetPasswordAsync(email);

            // Assert
            Assert.False(result.Success);

            // Verify no email is sent
            _kafkaProducerServiceMock.Verify(x => x.SendMessageAsync(It.IsAny<EmailForSendGrid>()), Times.Never);
        }

        [Fact]
        public async Task ResetPasswordAsync_InvalidTokenVerification_ReturnsFailure()
        {
            // Arrange
            var dto = new ResetPasswordDto { Token = "invalidToken", NewPassword = "newPass" };
            _passwordResetTokenServiceMock
                .Setup(x => x.VerifyPasswordResetToken(dto.Token))
                .Returns(new VerifyResetPasswordTokenResult
                {
                    Success = false,
                    Message = "Invalid token format"
                });

            // Act
            var result = await _service.ResetPasswordAsync(dto);

            // Assert
            Assert.False(result.Success);
            _passwordServiceMock.Verify(p => p.HashPassword(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task ResetPasswordAsync_UnregisteredToken_ReturnsFailure()
        {
            // Arrange
            var fakeToken = "validToken";
            var dto = new ResetPasswordDto { Token = fakeToken, NewPassword = "newPass" };
            var tokenVerificationResult = new VerifyResetPasswordTokenResult
            {
                Success = true,
                TokenId = Guid.NewGuid(),
                UserId = 1
            };

            _passwordResetTokenServiceMock
                .Setup(x => x.VerifyPasswordResetToken(dto.Token))
                .Returns(tokenVerificationResult);

            _tokenRepositoryMock
                .Setup(x => x.GetByIdAsync(tokenVerificationResult.TokenId))
                .ReturnsAsync((PasswordResetTokenInfo)null);

            // Act
            var result = await _service.ResetPasswordAsync(dto);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task ResetPasswordAsync_UserNotFound_ReturnsFailure()
        {
            // Arrange
            var fakeToken = "validToken";
            var dto = new ResetPasswordDto { Token = fakeToken, NewPassword = "newPass" };
            var tokenVerificationResult = new VerifyResetPasswordTokenResult
            {
                Success = true,
                TokenId = Guid.NewGuid(),
                UserId = 1
            };

            var tokenInfo = new PasswordResetTokenInfo();

            _passwordResetTokenServiceMock
                .Setup(x => x.VerifyPasswordResetToken(dto.Token))
                .Returns(tokenVerificationResult);

            _tokenRepositoryMock
                .Setup(x => x.GetByIdAsync(tokenVerificationResult.TokenId))
                .ReturnsAsync(tokenInfo);

            _usersRepositoryMock
                .Setup(x => x.GetByIdAsync(tokenVerificationResult.UserId))
                .ReturnsAsync((UserBase)null);

            // Act
            var result = await _service.ResetPasswordAsync(dto);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task ResetPasswordAsync_ValidRequest_ChangesPasswordAndReturnsSuccess()
        {
            // Arrange
            var fakeToken = "validToken";
            var newPassword = "newPass";
            var dto = new ResetPasswordDto { Token = fakeToken, NewPassword = newPassword };
            var tokenVerificationResult = new VerifyResetPasswordTokenResult
            {
                Success = true,
                TokenId = Guid.NewGuid(),
                UserId = 1
            };

            var tokenInfo = new PasswordResetTokenInfo();
            _passwordResetTokenServiceMock
                .Setup(x => x.VerifyPasswordResetToken(dto.Token))
                .Returns(tokenVerificationResult);

            _tokenRepositoryMock
                .Setup(x => x.GetByIdAsync(tokenVerificationResult.TokenId))
                .ReturnsAsync(tokenInfo);

            // Create a test user that captures the password change\n
            var user = new Customer { UserId = tokenVerificationResult.UserId, Email = "user@example.com" };
            _usersRepositoryMock
                .Setup(x => x.GetByIdAsync(tokenVerificationResult.UserId))
                .ReturnsAsync(user);

            var fakeHash = "hashedPassword";
            var fakeSalt = "salt";
            _passwordServiceMock
                .Setup(x => x.HashPassword(newPassword))
                .Returns((fakeHash, fakeSalt));

            _usersRepositoryMock
                .Setup(x => x.UpdateAsync(user))
                .Returns(Task.CompletedTask);

            _tokenRepositoryMock
                .Setup(x => x.DeleteTokenAsync(tokenInfo))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.ResetPasswordAsync(dto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(fakeHash, user.PasswordHash);
            Assert.Equal(fakeSalt, user.Salt);

            _usersRepositoryMock.Verify(x => x.UpdateAsync(user), Times.Once);
            _tokenRepositoryMock.Verify(x => x.DeleteTokenAsync(tokenInfo), Times.Once);
        }


    }
}
