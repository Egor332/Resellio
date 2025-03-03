using System;
using Xunit;
using FluentAssertions;
using ResellioBackend.UserManagementSystem.DTOs.Users;
using ResellioBackend.UserManagementSystem.Models.Base;
using ResellioBackend.UserManagementSystem.Models.Users;
using ResellioBackend.UserManagementSystem.Factories.Implementations;

namespace ResellioBackendTests.UserManagementSystemTests.FactoriesTests
{
    public class UserFactoryTests
    {
        private readonly UserFactory _factory;

        public UserFactoryTests()
        {
            _factory = new UserFactory();
        }

        [Fact]
        public void CreateNewUserWithoutPassword_ShouldReturnCustomer_ForRegisterCustomerDto()
        {
            // Arrange
            var dto = new RegisterCustomerDto
            {
                Email = "customer@example.com",
                FirstName = "John",
                LastName = "Doe"
            };

            // Act
            var result = _factory.CreateNewUserWithoutPassword(dto);

            // Assert            
            result.Should().BeOfType<Customer>();
            result.Email.Should().Be(dto.Email);
            result.FirstName.Should().Be(dto.FirstName);
            result.LastName.Should().Be(dto.LastName);
            result.CreatedDate.Should().Be(DateTime.Now.Date);
            result.IsActive.Should().BeTrue();
        }

        [Fact]
        public void CreateNewUserWithoutPassword_ShouldReturnOrganiser_ForRegisterOrganiserDto()
        {
            // Arrange
            var dto = new RegisterOrganiserDto
            {
                Email = "organiser@example.com",
                FirstName = "Jane",
                LastName = "Smith",
                OrganiserName = "name of organiser"
            };

            // Act
            var result = _factory.CreateNewUserWithoutPassword(dto);

            // Assert
            result.Should().BeOfType<Organiser>();
            result.Email.Should().Be(dto.Email);
            result.FirstName.Should().Be(dto.FirstName);
            result.LastName.Should().Be(dto.LastName);
            result.CreatedDate.Should().Be(DateTime.Now.Date);
            result.IsActive.Should().BeTrue();
            ((Organiser)result).IsVerified.Should().BeFalse();
            ((Organiser)result).OrganiserName.Should().Be(dto.OrganiserName);
        }
    }

}
