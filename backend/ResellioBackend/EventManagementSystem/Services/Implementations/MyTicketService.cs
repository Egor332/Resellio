using ResellioBackend.EventManagementSystem.Repositories.Abstractions;
using ResellioBackend.EventManagementSystem.Results;
using ResellioBackend.EventManagementSystem.Services.Abstractions;

namespace ResellioBackend.EventManagementSystem.Services.Implementations
{
    public class MyTicketService : IMyTicketService
    {
        private readonly IQRCodeService _qrCodeService;
        private readonly ITicketsRepository _ticketRepository;
        private readonly IQRCodeTemporaryCodeService _qRCodeTemporaryCodeService;

        public MyTicketService(IQRCodeService qrCodeService, ITicketsRepository ticketRepository,
            IQRCodeTemporaryCodeService qRCodeTemporaryCodeService)
        {
            _qrCodeService = qrCodeService;
            _ticketRepository = ticketRepository;
            _qRCodeTemporaryCodeService = qRCodeTemporaryCodeService;
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
            if (ticket.IsUsed)
            {
                return new QRCodeCreationResult()
                {
                    Success = false,
                    Message = "Ticket was already used"
                };
            }
            var result = await _qrCodeService.GenerateQRCodeAsync(ticketId);
            return result;
        }

        public async Task<bool> ValidateTicketAsync(Guid ticketId, Guid tempCode)
        {
            var tempCodeValidationResult = await _qRCodeTemporaryCodeService.CheckTemporaryCodeAsync(tempCode);
            if (!tempCodeValidationResult)
            {
                return false;
            }
            var ticket = await _ticketRepository.GetTicketByIdAsync(ticketId);
            if (ticket == null) 
            {
                return false;
            }
            var ticketSetUsedResult = ticket.MarkAsUsed();
            if (!ticketSetUsedResult)
            {
                return false;
            }
            await _ticketRepository.UpdateAsync(ticket);
            return true;

        }
    }
}
