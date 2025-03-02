using ResellioBackend.Models.Base;
using ResellioBackend.Results;
using System.ComponentModel.DataAnnotations;

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
    }
}
