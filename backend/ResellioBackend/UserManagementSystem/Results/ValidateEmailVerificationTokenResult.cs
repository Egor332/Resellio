using ResellioBackend.Results;

namespace ResellioBackend.UserManagementSystem.Results
{
    public class ValidateEmailVerificationTokenResult : ResultBase
    {
        public int UserId { get; set; }
    }
}
