using ResellioBackend.Results;
using ResellioBackend.UserManagementSystem.DTOs;

namespace ResellioBackend.UserManagementSystem.Services.Abstractions
{
    public interface IPasswordResetService
    {
        public Task<ResultBase> RequestPasswordResetAsync(string email);

        public Task<ResultBase> ChangePasswordAsync(ChangePasswordDto changePasswordDto);
    }
}
