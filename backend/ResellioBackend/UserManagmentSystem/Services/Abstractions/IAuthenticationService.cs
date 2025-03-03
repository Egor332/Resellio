using ResellioBackend.UserManagmentSystem.DTOs;
using ResellioBackend.UserManagmentSystem.Results;

namespace ResellioBackend.UserManagmentSystem.Services.Abstractions
{
    public interface IAuthenticationService
    {
        public Task<UserAuthenticationResult> LoginAsync(LoginCredentialsDto credentials);
    }
}
