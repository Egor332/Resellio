using ResellioBackend.Models.Base;
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
    }
}
