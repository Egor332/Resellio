using ResellioBackend.UserManagementSystem.Models.Base;
using ResellioBackend.UserManagementSystem.Statics;
using System.Security.Claims;

namespace ResellioBackend.UserManagementSystem.Models.Users
{
    public class Customer : UserBase
    {
        // TO ADD: List<Ticket>

        public override List<Claim> GetClaims()
        {
            var claims = GetBaseClaims();
            Claim roleClaim = new Claim(BearerTokenClaimsNames.Role, "Customer");
            claims.Add(roleClaim);

            return claims;
        }

        public override string GetRole()
        {
            return AuthorizationPolicies.CustomerPolicy;
        }
    }
}
