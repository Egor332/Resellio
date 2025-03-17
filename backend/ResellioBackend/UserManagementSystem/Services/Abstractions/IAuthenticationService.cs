using ResellioBackend.UserManagementSystem.DTOs;
using ResellioBackend.UserManagementSystem.Results;

namespace ResellioBackend.UserManagementSystem.Services.Abstractions
{
    public interface IAuthenticationService
    {
        public Task<UserAuthenticationResult> LoginAsync(LoginCredentialsDto credentials);
    }
}
