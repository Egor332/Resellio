using ResellioBackend.UserManagementSystem.DTOs.Base;
using ResellioBackend.UserManagementSystem.Results;

namespace ResellioBackend.UserManagementSystem.Services.Abstractions
{
    public interface IRegistrationService
    {
        public Task<RegistrationResult> RegisterUserAsync(RegisterUserDto dto);
    }
}
