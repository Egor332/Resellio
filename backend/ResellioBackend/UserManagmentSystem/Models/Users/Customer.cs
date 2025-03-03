using ResellioBackend.UserManagmentSystem.Models.Base;
using System.Security.Claims;

namespace ResellioBackend.UserManagmentSystem.Models.Users
{
    public class Customer : UserBase
    {
        // TO ADD: List<Ticket>

        public override List<Claim> GetClaims()
        {
            var claims = GetBaseClaims();
            Claim roleClaim = new Claim("Role", "Customer");
            claims.Add(roleClaim);

            return claims;
        }
    }
}
