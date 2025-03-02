using ResellioBackend.DTOs.Base;
using ResellioBackend.DTOs.Users;
using ResellioBackend.Factories.Abstractions;
using ResellioBackend.Models.Base;
using ResellioBackend.Models.Users;

namespace ResellioBackend.Factories.Implementations
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
                        Login = customerDto.Login,
                        FirstName = customerDto.FirstName,
                        LastName = customerDto.LastName,
                        CreatedDate = DateTime.Now.Date,
                    };
                    return newCustomer;
                case RegisterOrganiserDto organiserDto:
                    var newOrganiser = new Organiser
                    {
                        OrganiserName = organiserDto.OrganiserName,
                        Email = organiserDto.Email,
                        Login = organiserDto.Login,
                        FirstName = organiserDto.FirstName,
                        LastName = organiserDto.LastName,
                        CreatedDate = DateTime.Now.Date,
                        IsVerified = false
                    };
                    return newOrganiser;
                default:
                    throw new ArgumentException("Wrong dto format");
            }
        }
    }
}
