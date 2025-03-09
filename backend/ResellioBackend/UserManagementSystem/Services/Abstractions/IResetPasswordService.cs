using ResellioBackend.Results;
using ResellioBackend.UserManagementSystem.DTOs;

namespace ResellioBackend.UserManagementSystem.Services.Abstractions
{
    public interface IResetPasswordService
    {
        public Task<ResultBase> RequestResetPasswordAsync(string email);

        public Task<ResultBase> ResetPasswordAsync(ResetPasswordDto changePasswordDto);
    }
}
