using ResellioBackend.UserManagmentSystem.DTOs.Base;
using ResellioBackend.UserManagmentSystem.Results;

namespace ResellioBackend.UserManagmentSystem.Services.Abstractions
{
    public interface IRegistrationService
    {
        public Task<RegistrationResult> RegisterUserAsync(RegisterUserDto dto);
    }
}
