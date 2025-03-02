using ResellioBackend.DTOs.Base;
using ResellioBackend.Models.Base;

namespace ResellioBackend.Factories.Abstractions
{
    public interface IUserFactory
    {
        public UserBase CreateNewUserWithoutPassword(RegisterUserDto dto);
    }
}
