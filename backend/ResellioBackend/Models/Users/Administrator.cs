using ResellioBackend.Models.Base;
using System.Security.Claims;

namespace ResellioBackend.Models.Users
{
    public class Administrator : UserBase
    {
        public override List<Claim> GetClaims()
        {
            var claims = GetBaseClaims();
            Claim roleClaim = new Claim("Role", "Administrator");
            claims.Add(roleClaim);

            return claims;
        }
    }
}
