using ResellioBackend.UserManagementSystem.DTOs.Base;
using ResellioBackend.UserManagementSystem.Models.Base;

namespace ResellioBackend.UserManagementSystem.Factories.Abstractions
{
    public interface IUserFactory
    {
        public UserBase CreateNewUserWithoutPassword(RegisterUserDto dto);
    }
}
