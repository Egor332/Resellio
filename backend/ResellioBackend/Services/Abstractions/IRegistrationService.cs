using ResellioBackend.DTOs.Base;
using ResellioBackend.Results;

namespace ResellioBackend.Services.Abstractions
{
    public interface IRegistrationService
    {
        public Task<RegistrationResult> RegisterUserAsync(RegisterUserDto dto);
    }
}
