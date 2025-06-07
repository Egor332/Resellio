using ResellioBackend.EventManagementSystem.Results;

namespace ResellioBackend.EventManagementSystem.Services.Abstractions
{
    public interface IMyTicketService
    {
        public Task<QRCodeCreationResult> GetQRCodeAsync(int userId, Guid TicketId);

        public Task<bool> ValidateTicketAsync(Guid ticketId, Guid tempCode);
    }
}
