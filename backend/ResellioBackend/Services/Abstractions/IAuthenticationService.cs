using ResellioBackend.DTOs;
using ResellioBackend.Results;

namespace ResellioBackend.Services.Abstractions
{
    public interface IAuthenticationService
    {
        public Task<UserAuthenticationResult> LoginAsync(LoginCredentialsDto credentials);
    }
}
