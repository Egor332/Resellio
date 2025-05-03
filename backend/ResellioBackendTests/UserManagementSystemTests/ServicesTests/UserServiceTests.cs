using Moq;
using ResellioBackend.UserManagementSystem.DTOs.Base;
using ResellioBackend.UserManagementSystem.Models.Base;
using ResellioBackend.UserManagementSystem.Repositories.Abstractions;
using ResellioBackend.UserManagementSystem.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResellioBackendTests.UserManagementSystemTests.ServicesTests
{
    public class UserServiceTests
    {
        private readonly Mock<IUsersRepository<UserBase>> _mockUsersRepository;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockUsersRepository = new Mock<IUsersRepository<UserBase>>();
            _userService = new UserService(_mockUsersRepository.Object);
        }

        [Fact]
        public async Task GetUserInfoAsync_WhenUserExists_ReturnsSuccess()
        {
            // Arrange
            int userId = 1;
            var mockUser = new Mock<UserBase>();
            var expectedUserInfo = new UserInfoDto(); 
            mockUser.Setup(u => u.GetMyInfo()).Returns(expectedUserInfo);

            _mockUsersRepository.Setup(repo => repo.GetByIdAsync(userId))
                                .ReturnsAsync(mockUser.Object);

            // Act
            var result = await _userService.GetUserInfoAsync(userId);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(expectedUserInfo, result.userInfo);
        }

        [Fact]
        public async Task GetUserInfoAsync_WhenUserDoesNotExist_ReturnsFailure()
        {
            // Arrange
            int userId = 2;
            _mockUsersRepository.Setup(repo => repo.GetByIdAsync(userId))
                                .ReturnsAsync((UserBase?)null);

            // Act
            var result = await _userService.GetUserInfoAsync(userId);

            // Assert
            Assert.False(result.Success);
        }
    }
}
