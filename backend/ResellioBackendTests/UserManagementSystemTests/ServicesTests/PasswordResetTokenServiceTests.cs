using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using ResellioBackend.UserManagementSystem.Models.Tokens;
using ResellioBackend.UserManagementSystem.Repositories.Abstractions;
using ResellioBackend.UserManagementSystem.Services.Abstractions;
using ResellioBackend.UserManagementSystem.Services.Implementations;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ResellioBackendTests.UserManagementSystemTests.ServicesTests
{
    public class PasswordResetTokenServiceTests
    {
        private readonly Mock<IPasswordResetTokenRepository> _tokensRepositoryMock;
        private readonly Mock<ITokenGenerator> _tokenGeneratorMock;
        private readonly PasswordResetTokenService _service;
        private readonly byte[] _secretKey;

        public PasswordResetTokenServiceTests()
        {
            _tokensRepositoryMock = new Mock<IPasswordResetTokenRepository>();
            _tokenGeneratorMock = new Mock<ITokenGenerator>();

            var configurationMock = new Mock<IConfiguration>();
            configurationMock
                .Setup(config => config["JwtParameters:PasswordResetSecretKey"])
                .Returns("12345678123456781234567812345678");

            _secretKey = Encoding.UTF8.GetBytes("12345678123456781234567812345678");
            _service = new PasswordResetTokenService(_tokensRepositoryMock.Object, configurationMock.Object, _tokenGeneratorMock.Object);
        }

        [Fact]
        public async Task CreateTokenWithDatabaseRecordAsync_ShouldGenerateAndStoreNewToken()
        {
            // Arrange
            int userId = 123;
            var existingTokens = new List<PasswordResetTokenInfo>();
            _tokensRepositoryMock.Setup(r => r.GetUserTokensAsync(userId)).ReturnsAsync(existingTokens);

            var generatedToken = "mockToken";
            _tokenGeneratorMock.Setup(t => t.GenerateToken(It.IsAny<List<Claim>>(), 10, _secretKey, "", ""))
                                .Returns(generatedToken);

            // Act
            var result = await _service.CreateTokenWithDatabaseRecordAsync(userId);

            // Assert
            Assert.Equal(generatedToken, result);
            _tokensRepositoryMock.Verify(r => r.AddTokenAsync(It.IsAny<PasswordResetTokenInfo>()), Times.Once);
        }

        [Fact]
        public async Task CreateTokenWithDatabaseRecordAsync_ShouldGenerateAndStoreNewTokenAndDeletePreviousOnes()
        {
            int userId = 123;
            var existingTokens = new List<PasswordResetTokenInfo>() { new PasswordResetTokenInfo(), new PasswordResetTokenInfo()};
            _tokensRepositoryMock.Setup(r => r.GetUserTokensAsync(userId)).ReturnsAsync(existingTokens);
            _tokensRepositoryMock.Setup(r => r.DeleteTokenAsync(It.IsAny<PasswordResetTokenInfo>())).Returns(Task.CompletedTask);

            var generatedToken = "mockToken";
            _tokenGeneratorMock.Setup(t => t.GenerateToken(It.IsAny<List<Claim>>(), 10, _secretKey, "", ""))
                                .Returns(generatedToken);

            // Act
            var result = await _service.CreateTokenWithDatabaseRecordAsync(userId);

            // Assert
            Assert.Equal(generatedToken, result);
            _tokensRepositoryMock.Verify(r => r.AddTokenAsync(It.IsAny<PasswordResetTokenInfo>()), Times.Once);
            _tokensRepositoryMock.Verify(r => r.DeleteTokenAsync(It.IsAny<PasswordResetTokenInfo>()), Times.Exactly(existingTokens.Count));
        }

        [Fact]
        public void VerifyPasswordResetToken_ShouldReturnSuccessForValidToken()
        {
            // Arrange
            var tokenId = Guid.NewGuid();
            var userId = 456;

            var claims = new List<Claim>
        {
            new Claim(PasswordResetTokenService.IdClaimName, tokenId.ToString()),
            new Claim(PasswordResetTokenService.UserIdClaimName, userId.ToString())
        };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_secretKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var validToken = tokenHandler.WriteToken(securityToken);

            // Act
            var result = _service.VerifyPasswordResetToken(validToken);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(userId, result.UserId);
            Assert.Equal(tokenId, result.TokenId);
        }

        [Fact]
        public void VerifyPasswordResetToken_ShouldReturnFailureForInvalidSignature()
        {
            // Arrange
            var invalidToken = "invalid.token.here";

            // Act
            var result = _service.VerifyPasswordResetToken(invalidToken);

            // Assert
            Assert.False(result.Success);
        }
    }
}
