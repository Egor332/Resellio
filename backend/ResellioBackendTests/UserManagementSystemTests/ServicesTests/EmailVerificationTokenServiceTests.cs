using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
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
    public class EmailVerificationTokenServiceTests
    {
        private readonly Mock<ITokenGenerator> _tokenGeneratorMock;
        private readonly EmailVerificationTokenService _emailVerificationTokenService;
        private readonly byte[] _secretKey;
        private readonly int _testUserId = 123;

        public EmailVerificationTokenServiceTests()
        {
            _tokenGeneratorMock = new Mock<ITokenGenerator>();

            var configurationMock = new Mock<IConfiguration>();
            configurationMock
                .Setup(config => config["JwtParameters:EmailVerificationSecretKey"])
                .Returns("12345678123456781234567812345678");

            _secretKey = Encoding.UTF8.GetBytes("12345678123456781234567812345678");

            _emailVerificationTokenService = new EmailVerificationTokenService(
                _tokenGeneratorMock.Object,
                configurationMock.Object
            );
        }

        [Fact]
        public void EmailVerificationTokenService_GetEmailVerificationToken_ShouldGenerateToken_WithCorrectClaims()
        {
            // Arrange
            var expectedToken = "generated-token";
            _tokenGeneratorMock
                .Setup(x => x.GenerateToken(It.IsAny<List<Claim>>(), 60, _secretKey, "", ""))
                .Returns(expectedToken);

            // Act
            var result = _emailVerificationTokenService.GetEmailVerificationToken(_testUserId);

            // Assert
            Assert.Equal(expectedToken, result);
            _tokenGeneratorMock.Verify(x => x.GenerateToken(
                It.Is<List<Claim>>(claims =>
                    claims.Any(c => c.Type == "Id" && c.Value == _testUserId.ToString())),
                60, _secretKey, "", ""), Times.Once);
        }

        [Fact]
        public void EmailVerificationTokenService_ValidateEmailVerificationToken_ShouldReturnFailure_WhenTokenIsInvalid()
        {
            // Arrange
            var invalidToken = "invalid-token";

            // Act
            var result = _emailVerificationTokenService.ValidateEmailVerificationToken(invalidToken);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public void ValidateEmailVerificationToken_ShouldReturnFailure_WhenTokenHasNoIdClaim()
        {
            // Arrange
            var claims = new List<Claim>() 
            {
            new Claim("NotIdClaim", "123")
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(_secretKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Act
            var result = _emailVerificationTokenService.ValidateEmailVerificationToken(tokenHandler.WriteToken(token));

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public void ValidateEmailVerificationToken_ShouldReturnSuccess_WhenTokenIsValid()
        {
            // Arrange
            var claims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
            new Claim("Id", _testUserId.ToString())
            }));

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims.Identity as ClaimsIdentity,
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(_secretKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateEncodedJwt(tokenDescriptor);

            // Act
            var result = _emailVerificationTokenService.ValidateEmailVerificationToken(token);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(_testUserId, result.UserId);
        }

    }
}
