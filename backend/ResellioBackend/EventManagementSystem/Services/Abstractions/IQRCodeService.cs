using ResellioBackend.EventManagementSystem.Results;

namespace ResellioBackend.EventManagementSystem.Services.Abstractions
{
    public interface IQRCodeService
    {
        public Task<QRCodeCreationResult> GenerateQRCodeAsync(Guid ticketId);
        
    }
}
