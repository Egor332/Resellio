using ResellioBackend.EventManagementSystem.Services.Abstractions;

namespace ResellioBackend.EventManagementSystem.Services.Implementations
{
    public class QRCodeService : IQRCodeService
    {
        private readonly IQRCodeTemporaryCodeService _tempCodeService;

        public QRCodeService(IQRCodeTemporaryCodeService tempCodeService)
        {
            _tempCodeService = tempCodeService;
        }

        public async Task<byte[]> GenerateQRCodeAsync(Guid ticketId)
        {
            var temporaryCode = await _tempCodeService.CreateAndSaveTemporaryCodeAsync();
            var payload = new
            {
                TicketId = ticketId,
                TemporaryCode = temporaryCode,
            };
            throw new NotImplementedException();
        }
    }
}
