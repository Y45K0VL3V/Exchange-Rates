using yakov.ExchangeRates.Server.Domain.Entities;

namespace yakov.ExchangeRates.Server.Domain.Interfaces
{
    public interface IRatesRepository
    {
        public void Clear();

        public Task AddRates(List<Rate> rates);
        public Task<List<Rate>> GetRatesByCurrency(Currency currency);
        public Task<List<Rate>> GetPeriodRatesByCurrency(Currency currency, DateOnly dateStart, DateOnly dateEnd);

        public Dictionary<Currency, List<Rate>> GetAllRates();
    }
}
