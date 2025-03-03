using ResellioBackend.UserManagmentSystem.Models.Users;
using ResellioBackend.UserManagmentSystem.DTOs.Base;
using ResellioBackend.UserManagmentSystem.DTOs.Users;
using ResellioBackend.UserManagmentSystem.Factories.Abstractions;
using ResellioBackend.UserManagmentSystem.Models.Base;

namespace ResellioBackend.UserManagmentSystem.Factories.Implementations
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
                        IsActive = true
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
                        IsActive = true,
                        IsVerified = false
                    };
                    return newOrganiser;
                default:
                    throw new ArgumentException("Wrong dto format");
            }
        }
    }
}
