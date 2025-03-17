using ResellioBackend.UserManagementSystem.Models.Users;
using ResellioBackend.UserManagementSystem.DTOs.Base;
using ResellioBackend.UserManagementSystem.DTOs.Users;
using ResellioBackend.UserManagementSystem.Factories.Abstractions;
using ResellioBackend.UserManagementSystem.Models.Base;

namespace ResellioBackend.UserManagementSystem.Factories.Implementations
{
    public class UserFactory : IUserFactory
    {
        public UserBase CreateNewUserWithoutPassword(RegisterUserDto dto)
        {
            switch (dto)
            {
                case RegisterCustomerDto customerDto:
                    var newCustomer = new Customer
                    {
                        Email = customerDto.Email,
                        FirstName = customerDto.FirstName,
                        LastName = customerDto.LastName,
                        CreatedDate = DateTime.Now.Date,
                        IsActive = false
                    };
                    return newCustomer;
                case RegisterOrganiserDto organiserDto:
                    var newOrganiser = new Organiser
                    {
                        OrganiserName = organiserDto.OrganiserName,
                        Email = organiserDto.Email,
                        FirstName = organiserDto.FirstName,
                        LastName = organiserDto.LastName,
                        CreatedDate = DateTime.Now.Date,
                        IsActive = false,
                        IsVerified = false
                    };
                    return newOrganiser;
                default:
                    throw new ArgumentException("Wrong dto format");
            }
        }
    }
}
