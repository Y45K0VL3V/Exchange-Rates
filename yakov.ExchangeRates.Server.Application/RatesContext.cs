using yakov.ExchangeRates.Server.Domain.Entities;

namespace yakov.ExchangeRates.Server.Application
{
    public class RatesContext
    {
        public RatesContext()
        {
            Rates = new();
        }

        public Dictionary<Currency, List<Rate>> Rates { get; set; }
    }
}
