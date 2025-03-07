using ResellioBackend.Results;

namespace ResellioBackend.UserManagementSystem.Services.Abstractions
{
    public interface IRequestEmailVerificationService
    {
        public Task<ResultBase> ResentEmailVerificationMessageAsync(string email);
    }
}
