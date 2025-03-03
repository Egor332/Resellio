using ResellioBackend.UserManagmentSystem.DTOs.Base;
using ResellioBackend.UserManagmentSystem.Models.Base;

namespace ResellioBackend.UserManagmentSystem.Factories.Abstractions
{
    public interface IUserFactory
    {
        public UserBase CreateNewUserWithoutPassword(RegisterUserDto dto);
    }
}
