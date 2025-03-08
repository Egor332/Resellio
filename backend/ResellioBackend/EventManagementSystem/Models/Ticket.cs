using System;using System.ComponentModel.DataAnnotations;
using ResellioBackend.UserManagementSystem.Models.Users;

namespace ResellioBackend.EventManagementSystem.Models.Base
{
    public class Ticket
    {
        // TODO: add other properties
        
        [Key]
        public Guid TicketId { get; set; }

        [Required]
        public int TicketTypeId { get; set; }
        [Required]
        public TicketType TicketType { get; set; }

        public int? OwnerId { get; set; }
        
        public Customer? Owner { get; set; }
    }
}