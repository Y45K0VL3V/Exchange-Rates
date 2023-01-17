using yakov.ExchangeRates.Server.Domain.Entities;
using yakov.ExchangeRates.Server.Domain.Interfaces;

namespace yakov.ExchangeRates.Server.Infrastructure
{
    public class CacheService : ICacheService
    {
        public CacheService(IAPIServiceBuilder apiServiceBuilder, ISavedRatesLoaderService savedRatesLoaderService, IRatesRepository ratesRepository, ITimePeriodValidator timePeriodValidator)
        {
            _apiServiceBuilder = apiServiceBuilder;
            _savedRatesLoaderService = savedRatesLoaderService;
            _ratesRepository = ratesRepository;
            _timePeriodValidator = timePeriodValidator;

            _savedRatesLoaderService.LoadAll().Wait();
        }

        private IAPIServiceBuilder _apiServiceBuilder;
        private ISavedRatesLoaderService _savedRatesLoaderService;
        private IRatesRepository _ratesRepository;
        private ITimePeriodValidator _timePeriodValidator;

        private async Task<List<Rate>> GetMissedRates(List<DateOnly> missedDates, IRatesAPIService ratesAPI, Currency currency)
        {
            List<Rate> rates = new List<Rate>();
            List<Rate>? gotRates = null;
            var streakDateStart = missedDates.First();
            var streakDateEnd = streakDateStart;
            var missedDatesStreak = 1;
            for (int i = 1; i < missedDates.Count; i++)
            {
                if ((missedDates[i] == streakDateEnd.AddDays(1)) && (missedDatesStreak++ != 100))
                    streakDateEnd = missedDates[i];
                else
                {
                    gotRates = await ratesAPI.GetRatesByTimePeriod(currency, streakDateStart, streakDateEnd);
                    rates.AddRange(gotRates);
                    gotRates = null;

                    streakDateStart = missedDates[i];
                    streakDateEnd = streakDateStart;
                    missedDatesStreak = 1;
                }
            }

            if (gotRates is null)
            {
                gotRates = await ratesAPI.GetRatesByTimePeriod(currency, streakDateStart, streakDateEnd);
                rates.AddRange(gotRates);
            }

            await _ratesRepository.AddRates(rates);

            return rates;
        }

        public async Task<List<Rate>> GetRatesWithPeriod(Currency currency, DateOnly dateStart, DateOnly dateEnd)
        {
            _timePeriodValidator.Validate(ref dateStart, ref dateEnd);
            var cachedRates = _ratesRepository.GetPeriodRatesByCurrency(currency, dateStart, dateEnd);
            var targetRatesAmount = (dateEnd.ToDateTime(new()) - dateStart.ToDateTime(new())).Days + 1;

            var rates = await cachedRates ?? new();
            if (rates.Count < targetRatesAmount)
            {
                var routingAPI = _apiServiceBuilder.BuildAPIService(currency.Type);

                List<DateOnly> missedDates = new();
                for (DateOnly currDate = dateStart; currDate <= dateEnd; currDate = currDate.AddDays(1))
                {
                    if (!rates.Any(r => r.Date == currDate))
                        missedDates.Add(currDate);
                }

                var missedRates = await GetMissedRates(missedDates, routingAPI, currency);
                rates.AddRange(missedRates ?? new());
            }

            _savedRatesLoaderService.SaveByCurrency(currency);

            rates.Sort((x, y) => x.Date.CompareTo(y.Date));
            return rates;
        }
    }
}
