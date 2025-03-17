using ResellioBackend.Results;

namespace ResellioBackend.UserManagementSystem.Services.Abstractions
{
    public interface IConfirmEmailService
    {
        public Task<ResultBase> ConfirmEmailAsync(string token);
    }
}
