using ResellioBackend.Results;

namespace ResellioBackend.UserManagementSystem.Results
{
    public class UserAuthenticationResult : ResultBase
    {
        public string Token { get; set; }
        public string UserRole { get; set; }
    }
}
