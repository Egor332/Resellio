using ResellioBackend.Statics;

namespace ResellioBackend.EventManagementSystem.Models
{
    public class Money
    {
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }

        public bool SetPrice(decimal amount, string CurrencyCode)
        {
            this.Amount = amount;
            if (CurrencyCodes.AvailableCurrencies.Contains(CurrencyCode))
            {
                this.CurrencyCode = CurrencyCode;
                return true;
            }
            return false;
        }
    }
}
