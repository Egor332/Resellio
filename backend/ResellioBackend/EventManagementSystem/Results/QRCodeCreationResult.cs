using ResellioBackend.Results;

namespace ResellioBackend.EventManagementSystem.Results
{
    public class QRCodeCreationResult : ResultBase
    {
        public byte[] QRCodeImage { get; set; }
    }
}
