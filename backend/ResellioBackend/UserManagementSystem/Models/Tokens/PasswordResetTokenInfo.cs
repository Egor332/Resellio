using ResellioBackend.UserManagementSystem.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResellioBackend.UserManagementSystem.Models.Tokens
{
    public class PasswordResetTokenInfo
    {
        [Key]
        public Guid Id { get; set; }

        public int OwnerId { get; set; }

        [ForeignKey("OwnerId")]
        public UserBase Owner { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
