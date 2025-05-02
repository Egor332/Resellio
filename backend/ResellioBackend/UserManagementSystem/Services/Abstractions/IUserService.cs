using ResellioBackend.UserManagementSystem.Results;

namespace ResellioBackend.UserManagementSystem.Services.Abstractions
{
    public interface IUserService
    {
        public Task<GetUserInfoResult> GetUserInfoAsync(int userId);
    }
}
