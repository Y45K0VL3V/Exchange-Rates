using yakov.ExchangeRates.Server.Domain.Entities;

namespace yakov.ExchangeRates.Server.Domain.Interfaces
{
    public interface IRatesRepository
    {
        public Task AddRates(List<Rate> rates);
        public Dictionary<Currency, List<Rate>> GetAllRates();
        public Task<List<Rate>> GetAllRatesByCurrency(Currency currency);
        public Task<List<Rate>> GetPeriodRatesByCurrency(Currency currency, DateOnly dateStart, DateOnly dateEnd);
    }
}
