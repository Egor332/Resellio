using Microsoft.AspNetCore.Authorization.Infrastructure;
using ResellioBackend.Results;
using ResellioBackend.UserManagementSystem.DTOs.Base;
using ResellioBackend.UserManagementSystem.Models.Base;
using ResellioBackend.UserManagementSystem.Statics;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace ResellioBackend.UserManagementSystem.Models.Users
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
            Claim roleClaim = new Claim(BearerTokenClaimsNames.Role, "Organiser");
            claims.Add(roleClaim);

            return claims;
        }

        public override string GetRole()
        {
            return AuthorizationPolicies.OrganiserPolicy;
        }

        public override UserInfoDto GetMyInfo()
        {
            return new UserInfoDto()
            {
                Email = this.Email,
                FirstName = this.FirstName,
                LastName = this.LastName,
                CreatedDate = this.CreatedDate,
                OrganiserName = this.OrganiserName,
                ConfirmedSeller = !(this.ConnectedSellingAccount == null)
            };
        }
    }
}
