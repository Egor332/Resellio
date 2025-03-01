using ResellioBackend.Services.Implementations;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ResellioBackendTests.ServicesTests
{
    public class JwtGeneratorTests
    {
        private readonly JwtGenerator _jwtGenerator = new JwtGenerator();
        private readonly byte[] _key = Encoding.ASCII.GetBytes("12345678123456781234567812345678"); // should have 32 chars
        private const string Issuer = "https://backend.com";
        private const string Audience = "https://target.com";

        [Fact]
        public void JwtGenerator_GenerateToken_ReturnsNotEmptyJwt()
        {
            // Arrange
            var claims = new List<Claim> { new Claim(JwtRegisteredClaimNames.Name, "testuser") };
            int expirationInMinutes = 60;

            // Act
            var token = _jwtGenerator.GenerateToken(claims, expirationInMinutes, _key, Issuer, Audience);

            // Assert
            Assert.False(string.IsNullOrEmpty(token), "Generated token should not be null or empty.");
        }

        [Fact]
        public void JwtGenerator_GenerateToken_ContainsCorrectClaims()
        {
            // Arrange
            var claims = new List<Claim> { new Claim(JwtRegisteredClaimNames.Name, "testuser") };
            int expirationInMinutes = 60;

            // Act
            var token = _jwtGenerator.GenerateToken(claims, expirationInMinutes, _key, Issuer, Audience);
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Assert
            Assert.Contains(jwtToken.Claims, c => c.Type == JwtRegisteredClaimNames.Name && c.Value == "testuser");            
        }

        [Fact]
        public void JwtGenerator_GenerateToken_SetsCorrectIssuerAndAudience()
        {
            // Arrange
            var claims = new List<Claim> { new Claim(JwtRegisteredClaimNames.Name, "testuser") };
            int expirationInMinutes = 60;

            // Act
            var token = _jwtGenerator.GenerateToken(claims, expirationInMinutes, _key, Issuer, Audience);
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Assert
            Assert.Equal(Issuer, jwtToken.Issuer);
            Assert.Equal(Audience, jwtToken.Audiences.FirstOrDefault());
        }

        [Fact]
        public void JwtGenerator_GenerateToken_SetsExpirationCorrectly()
        {
            // Arrange
            var claims = new List<Claim> { new Claim(JwtRegisteredClaimNames.Name, "testuser") };
            int expirationInMinutes = 5;

            // Act
            var token = _jwtGenerator.GenerateToken(claims, expirationInMinutes, _key, Issuer, Audience);
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Assert
            Assert.NotNull(jwtToken.ValidTo);
            Assert.True(jwtToken.ValidTo <= DateTime.UtcNow.AddMinutes(expirationInMinutes));
        }

        [Fact]
        public void JwtGenerator_GenerateToken_WithEmptyClaims_ShouldNotThrowException()
        {
            // Arrange
            var claims = new List<Claim>();
            int expirationInMinutes = 60;

            // Act
            var token = _jwtGenerator.GenerateToken(claims, expirationInMinutes, _key, Issuer, Audience);

            // Assert
            Assert.False(string.IsNullOrEmpty(token));
        }

        [Fact]
        public void JwtGenerator_GenerateToken_WithNullIssuerAndAudience_ShouldNotThrowException()
        {
            // Arrange
            var claims = new List<Claim> { new Claim(JwtRegisteredClaimNames.Name, "testuser") };
            int expirationInMinutes = 60;

            // Act
            var token = _jwtGenerator.GenerateToken(claims, expirationInMinutes, _key, null, null);

            // Assert
            Assert.False(string.IsNullOrEmpty(token));
        }
    }
}
