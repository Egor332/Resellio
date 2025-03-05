using System;using System.ComponentModel.DataAnnotations;
using ResellioBackend.UserManagementSystem.Models.Users;

namespace ResellioBackend.EventManagementSystem.Models.Base
{
    public class Ticket
    {
        // TODO: add other properties
        
        [Key]
        public int TicketId { get; set; }

        [Required]
        public TicketType TicketType { get; set; }

        public Customer? Owner { get; set; }
    }
}