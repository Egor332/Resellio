using ResellioBackend.EventManagementSystem.Repositories.Abstractions;
using ResellioBackend.EventManagementSystem.Services.Abstractions;

namespace ResellioBackend.EventManagementSystem.Services.Implementations
{
    public class QRCodeTemporaryCodeService : IQRCodeTemporaryCodeService
    {
        private readonly IQRCodeTemporaryCodeRepository _qrCodeTemporaryCodeRepository;

        public QRCodeTemporaryCodeService(IQRCodeTemporaryCodeRepository qrCodeTemporaryCodeRepository)
        {
            _qrCodeTemporaryCodeRepository = qrCodeTemporaryCodeRepository;
        }

        public async Task<Guid?> CreateAndSaveTemporaryCodeAsync()
        {
            var newCode = Guid.NewGuid();
            var isCodeAdded = await _qrCodeTemporaryCodeRepository.AddTemporaryCodeAsync(newCode);
            if (isCodeAdded)
            {
                return null;
            }
            return newCode;
        }

        public async Task<bool> CheckTemporaryCodeAsync(Guid code)
        {
            var isCodeVerified = await _qrCodeTemporaryCodeRepository.IsTemporaryCodeExistAsync(code);
            if (isCodeVerified) 
            {
                await _qrCodeTemporaryCodeRepository.RemoveTemporaryCodeAsync(code);
            }
            return isCodeVerified;
        }
    }
}
