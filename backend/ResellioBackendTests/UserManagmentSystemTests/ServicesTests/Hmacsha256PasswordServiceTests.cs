using ResellioBackend.UserManagmentSystem.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResellioBackendTests.UserManagmentSystemTests.ServicesTests
{
    public class Hmacsha256PasswordServiceTests
    {
        private readonly Hmacsha256PasswordService _passwordService;

        public Hmacsha256PasswordServiceTests()
        {
            _passwordService = new Hmacsha256PasswordService();
        }

        [Fact]
        public void Hmacsha256PasswordService_GetHashPassword_ShouldReturnNonEmptyHashAndSlat()
        {
            // Arrange
            string password = "password123";

            // Act
            (string hash, string salt) = _passwordService.HashPassword(password);

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(hash), "Hash should not be null or empty");
            Assert.False(string.IsNullOrWhiteSpace(salt), "Salt should not be null or empty");
        }

        [Fact]
        public void Hmacsha256PasswordService_VerifyPassword_ShouldReturnTrueForCorrectPassword()
        {
            // Arrange
            string password = "password123";
            (string hash, string salt) = _passwordService.HashPassword(password);

            // Act
            bool result = _passwordService.VerifyPassword(password, hash, salt);

            // Assert
            Assert.True(result, "VerifyPassword should return true for correct password");
        }

        [Fact]
        public void Hmacsha256PasswordService_VerifyPassword_ShouldReturnFlaseForCorrectPassword()
        {
            // Arrange
            string password = "password123";
            (string hash, string salt) = _passwordService.HashPassword(password);
            string wrongPassword = "wrongPassword123";

            // Act
            bool result = _passwordService.VerifyPassword(wrongPassword, hash, salt);

            // Assert
            Assert.False(result, "VerifyPassword should return false for wrong password");
        }

        [Fact]
        public void Hmacsha256PasswordService_HashPassword_ShouldGenerateDifferentHashesForSamePassword()
        {
            // Arrange
            string password = "password123";

            // Act
            (string hash1, string salt1) = _passwordService.HashPassword(password);
            (string hash2, string salt2) = _passwordService.HashPassword(password);

            // Assert
            Assert.NotEqual(hash1, hash2);
            Assert.NotEqual(salt1, salt2);
        }
    }
}
