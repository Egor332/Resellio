using ResellioBackend.EventManagementSystem.Repositories.Abstractions;
using ResellioBackend.EventManagementSystem.Results;
using ResellioBackend.EventManagementSystem.Services.Abstractions;

namespace ResellioBackend.EventManagementSystem.Services.Implementations
{
    public class MyTicketService : IMyTicketService
    {
        private readonly IQRCodeService _qrCodeService;
        private readonly ITicketsRepository _ticketRepository;

        public MyTicketService(IQRCodeService qrCodeService, ITicketsRepository ticketRepository)
        {
            _qrCodeService = qrCodeService;
            _ticketRepository = ticketRepository;
        }

        public async Task<QRCodeCreationResult> GetQRCodeAsync(int userId, Guid ticketId)
        {
            var ticket = await _ticketRepository.GetTicketByIdAsync(ticketId);
            if (ticket == null) 
            {
                return new QRCodeCreationResult()
                {
                    Success = false,
                    Message = "This ticket does not exist"
                };
            }
            if (ticket.HolderId != userId)
            {
                return new QRCodeCreationResult()
                {
                    Success = false,
                    Message = "Unauthorize to use this ticket"
                };
            }
            var result = await _qrCodeService.GenerateQRCodeAsync(ticketId);
            return result;
        }
    }
}
