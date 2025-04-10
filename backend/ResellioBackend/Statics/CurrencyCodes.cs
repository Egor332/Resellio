namespace ResellioBackend.Statics
{
    public static class CurrencyCodes
    {
        public const string USD = "USD";
        public const string EUR = "EUR";
        public const string GBP = "GBP";
        public const string PLN = "PLN";

        public static readonly HashSet<string> AvailableCurrencies = new()
        {
            USD, EUR, GBP, PLN
        };
    }
}
