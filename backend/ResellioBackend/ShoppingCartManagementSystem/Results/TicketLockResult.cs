using ResellioBackend.Results;
using System.Security.Cryptography.X509Certificates;

namespace ResellioBackend.ShoppingCartManagementSystem.Results
{
    public class TicketLockResult : ResultBase
    {
        public Guid TicketId { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}
