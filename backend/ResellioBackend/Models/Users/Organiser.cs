using Microsoft.AspNetCore.Authorization.Infrastructure;
using ResellioBackend.Models.Base;
using ResellioBackend.Results;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace ResellioBackend.Models.Users
{
    public class Organiser : UserBase
    {
        // TO ADD List<Event>

        [Required]
        [MaxLength(100)]
        public string OrganiserName { get; set; }

        [Required]
        public bool IsVerified { get; set; }

        public override ResultBase ValidateAccount()
        {
            ResultBase activeVerification = base.ValidateAccount();
            if (!activeVerification.Success)
            {
                return activeVerification;
            }
            if (IsVerified)
            {
                return new ResultBase()
                {
                    Success = true,
                    Message = "",
                };
            }
            else
            {
                return new ResultBase()
                {
                    Success = false,
                    Message = "Your account have not been verified yet"
                };
            }
        }

        public override List<Claim> GetClaims()
        {
            var claims = GetBaseClaims();
            Claim roleClaim = new Claim("Role", "Organiser");
            claims.Add(roleClaim);

            return claims;
        }
    }
}
