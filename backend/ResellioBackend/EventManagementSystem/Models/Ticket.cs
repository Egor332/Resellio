using System.ComponentModel.DataAnnotations;
using ResellioBackend.EventManagementSystem.Enums;
using ResellioBackend.UserManagementSystem.Models.Base;
using ResellioBackend.UserManagementSystem.Models.Users;

namespace ResellioBackend.EventManagementSystem.Models.Base
{
    public class Ticket
    {        
        [Key]
        public Guid TicketId { get; set; }
        [Required]
        public int TicketTypeId { get; set; }
        [Required]
        public TicketType TicketType { get; set; }
        public Money? CurrentPrice { get; set; }
        [Required]
        public TicketStates TicketState { get; set; }
        public DateTime? LastLock { get; set; }
        public int? PurchaseIntenderId { get; set; }        
        public Customer? PurchaseIntender { get; set; }
        public int HolderId { get; set; }
        public UserBase Holder { get; set; }

        [Required]
        public bool IsUsed { get; set; } = false;

        public void ChangeLockParameters(DateTime? newLock, TicketStates newStatus, int? intenderId)
        {
            LastLock = newLock;
            TicketState = newStatus;
            PurchaseIntenderId = intenderId;
        }

        public Money? GetPrice()
        {
            if (CurrentPrice != null)
            {
                return new Money() 
                {
                    Amount = CurrentPrice.Amount,
                    CurrencyCode = CurrentPrice.CurrencyCode,
                };
            }
            if (this.TicketType != null)
            {
                return new Money()
                {
                    Amount = TicketType!.BasePrice.Amount,
                    CurrencyCode = TicketType.BasePrice.CurrencyCode,
                };
            }
            else
            {
                return null;
            }
        }

        public bool PutTicketOnSale(decimal amount, string currency)
        {
            var price = new Money();
            if (!price.SetPrice(amount, currency))
            {
                return false;
            }

            this.CurrentPrice = price;
            LastLock = null;
            PurchaseIntender = null;
            PurchaseIntenderId = null;
            TicketState = TicketStates.Available;

            return true;
        }

        public void StopSellingTicket()
        {
            this.CurrentPrice = null;
            TicketState = TicketStates.Sold;
        }

        public bool MarkAsUsed()
        {
            if (IsUsed)
            {
                return false;
            }
            IsUsed = true;
            return true;
        }
    }
}