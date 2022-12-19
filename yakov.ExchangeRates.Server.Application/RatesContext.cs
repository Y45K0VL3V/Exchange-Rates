using System.Collections.Concurrent;
using yakov.ExchangeRates.Server.Domain.Entities;

namespace yakov.ExchangeRates.Server.Application
{
    public class RatesContext
    {
        public RatesContext()
        {
            Rates = new();
        }

        public ConcurrentDictionary<Currency, List<Rate>> Rates { get; set; }
    }
}
