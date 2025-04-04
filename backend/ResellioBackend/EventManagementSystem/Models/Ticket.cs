using System;
using System.ComponentModel.DataAnnotations;
using ResellioBackend.EventManagementSystem.Enums;
using ResellioBackend.UserManagementSystem.Models.Base;
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

        [Required]
        public TicketStates TicketState { get; set; }

        public DateTime? LastLock { get; set; }

        public int? OwnerId { get; set; }
        
        public Customer? Owner { get; set; }

        public int SellerId { get; set; }
        public UserBase Seller { get; set; }
    }
}