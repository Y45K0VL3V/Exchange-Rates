using yakov.ExchangeRates.Server.Domain.Interfaces;

namespace yakov.ExchangeRates.Server.Infrastructure.FileServices
{
    public class SavedRatesLoaderService : ISavedRatesLoaderService
    {
        public SavedRatesLoaderService(IRatesFileService ratesFileService, IRatesRepository ratesRepository)
        {
            _ratesRepository = ratesRepository;
            _ratesFileService = ratesFileService;
        }

        private IRatesFileService _ratesFileService;
        private IRatesRepository _ratesRepository;
        
        public async Task LoadAll()
        {
            var currencyToRates = await _ratesFileService.GetSavedRates();
            foreach (var rates in currencyToRates)
            {
                await _ratesRepository.AddRates(rates.Value);
            }
        }

        public async Task SaveAll()
        {
            var currencyRates = _ratesRepository.GetAllRates();
            foreach (var currRates in currencyRates)
            {
                await _ratesFileService.WriteRatesByCurrency(currRates.Value, currRates.Key);
            }
        }
    }
}
