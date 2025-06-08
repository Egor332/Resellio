using Newtonsoft.Json;
using QRCoder;
using ResellioBackend.EventManagementSystem.Results;
using ResellioBackend.EventManagementSystem.Services.Abstractions;
using System.Text.Json;

namespace ResellioBackend.EventManagementSystem.Services.Implementations
{
    public class QRCodeService : IQRCodeService
    {
        private readonly IQRCodeTemporaryCodeService _tempCodeService;

        public QRCodeService(IQRCodeTemporaryCodeService tempCodeService)
        {
            _tempCodeService = tempCodeService;
        }

        public async Task<QRCodeCreationResult> GenerateQRCodeAsync(Guid ticketId)
        {
            var temporaryCode = await _tempCodeService.CreateAndSaveTemporaryCodeAsync();
            if (temporaryCode == null)
            {
                return new QRCodeCreationResult()
                {
                    Success = false,
                    Message = "Something went wrong please try again"
                };
            }
            var payload = new
            {
                TicketId = ticketId,
                TemporaryCode = temporaryCode,
            };

            string jsonPayload = JsonConvert.SerializeObject(payload);
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(jsonPayload, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new PngByteQRCode(qrCodeData);
            var qrCodeImage = qrCode.GetGraphic(10);

            

            return new QRCodeCreationResult {
                Success = true,
                QRCodeImage = qrCodeImage
            };
        }
    }
}
