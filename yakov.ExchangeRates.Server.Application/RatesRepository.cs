﻿using yakov.ExchangeRates.Server.Domain.Entities;
using yakov.ExchangeRates.Server.Domain.Interfaces;

namespace yakov.ExchangeRates.Server.Application
{
    public class RatesRepository : IRatesRepository
    {
        public RatesRepository(RatesContext ratesRepository)
        {
            _ratesContext = ratesRepository;
        }

        private RatesContext _ratesContext;

        public void Clear()
        {
            _ratesContext.Rates = new();
        }

        public async Task AddRates(List<Rate> rates)
        {
            await Task.Run(() =>
            {
                foreach (var rate in rates)
                {
                    if (!_ratesContext.Rates.ContainsKey(rate.Currency))
                        _ratesContext.Rates.TryAdd(rate.Currency, new());

                    try
                    {
                        _ratesContext.Rates[rate.Currency].Add(rate);
                    }
                    catch { throw; }
                }
            });
        }

        public async Task<List<Rate>> GetRatesByCurrency(Currency currency)
        {
            List<Rate> rates = new();
            await Task.Run(() =>
            {
                if (_ratesContext.Rates.ContainsKey(currency))
                    rates.AddRange(_ratesContext.Rates[currency] ?? new());
            });
            
            return rates;
        }

        public async Task<List<Rate>> GetPeriodRatesByCurrency(Currency currency, DateOnly dateStart, DateOnly dateEnd)
        {
            return (await GetRatesByCurrency(currency))
                .Where(r => r.Date >= dateStart && r.Date <= dateEnd).ToList();
        }

        public Dictionary<Currency, List<Rate>> GetAllRates() => _ratesContext.Rates.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }
}
