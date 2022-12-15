using yakov.ExchangeRates.Server.Domain.Entities;

namespace yakov.ExchangeRates.Server.Application
{
    public class RatesContext
    {
        public RatesContext()
        {
            Rates = new();
        }

        public Dictionary<Currency, Rate> Rates { get; set; }
    }
}
