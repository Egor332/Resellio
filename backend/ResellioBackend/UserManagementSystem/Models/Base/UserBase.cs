using ResellioBackend.Results;
using ResellioBackend.UserManagementSystem.DTOs.Base;
using ResellioBackend.UserManagementSystem.Statics;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace ResellioBackend.UserManagementSystem.Models.Base
{
    public abstract class UserBase
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string Salt { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(60)]
        public string LastName { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public string? ConnectedSellingAccount {  get; set; }

        public abstract List<Claim> GetClaims();

        public abstract string GetRole();

        public virtual ResultBase ValidateAccount()
        {
            if (IsActive)
            {
                return new ResultBase()
                {
                    Success = true,
                    Message = ""
                };
            }
            else
            {
                return new ResultBase()
                {
                    Success = false,
                    Message = "This email was not confirmed"
                };
            }
        }

        public void ChangePassword(string newPasswordHash, string newSalt)
        {
            PasswordHash = newPasswordHash;
            Salt = newSalt;
        }

        protected List<Claim> GetBaseClaims()
        {
            List<Claim> claims = new List<Claim>();
            Claim emailClaim = new Claim(BearerTokenClaimsNames.Email, Email);
            Claim idClaim = new Claim(BearerTokenClaimsNames.Id, UserId.ToString());
            claims.Add(emailClaim);
            claims.Add(idClaim);
            return claims;
        }

        public virtual UserInfoDto GetMyInfo()
        {
            return new UserInfoDto()
            {
                Email = this.Email,
                FirstName = this.FirstName,
                LastName = this.LastName,
                CreatedDate = this.CreatedDate,
                ConfirmedSeller = !(this.ConnectedSellingAccount == null),
                Role = GetRole()
            };
        }

        public bool ValidateAbilityToSale()
        {
            if (string.IsNullOrEmpty(this.ConnectedSellingAccount))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
