using System;
using Xunit;
using FluentAssertions;
using ResellioBackend.DTOs.Users;
using ResellioBackend.Factories.Implementations;
using ResellioBackend.Models.Users;
using ResellioBackend.Models.Base;

namespace ResellioBackendTests.FactoriesTests
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
                Login = "customerLogin",
                FirstName = "John",
                LastName = "Doe"
            };

            // Act
            var result = _factory.CreateNewUserWithoutPassword(dto);

            // Assert            
            result.Should().BeOfType<Customer>();
            result.Email.Should().Be(dto.Email);
            result.Login.Should().Be(dto.Login);
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
                Login = "organiserLogin",
                FirstName = "Jane",
                LastName = "Smith",
                OrganiserName = "name of organiser"
            };

            // Act
            var result = _factory.CreateNewUserWithoutPassword(dto);

            // Assert
            result.Should().BeOfType<Organiser>();
            result.Email.Should().Be(dto.Email);
            result.Login.Should().Be(dto.Login);
            result.FirstName.Should().Be(dto.FirstName);
            result.LastName.Should().Be(dto.LastName);
            result.CreatedDate.Should().Be(DateTime.Now.Date);
            result.IsActive.Should().BeTrue();
            ((Organiser)result).IsVerified.Should().BeTrue();
            ((Organiser)result).OrganiserName.Should().Be(dto.OrganiserName);
        }
    }

}
