using ResellioBackend.Results;

namespace ResellioBackend.UserManagementSystem.Results
{
    public class VerifyResetPasswordTokenResult : ResultBase
    {
        public int UserId { get; set; }
        public Guid TokenId { get; set; }
    }
}
