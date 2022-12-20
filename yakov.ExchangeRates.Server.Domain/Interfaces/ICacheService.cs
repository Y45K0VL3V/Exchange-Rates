using yakov.ExchangeRates.Server.Domain.Entities;

namespace yakov.ExchangeRates.Server.Domain.Interfaces
{
    public interface ICacheService
    {
        public Task<List<Rate>> GetRatesWithPeriod(Currency currency, DateOnly dateStart, DateOnly dateEnd);
    }
}
