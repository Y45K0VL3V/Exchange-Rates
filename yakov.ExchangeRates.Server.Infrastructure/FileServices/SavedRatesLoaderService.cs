using yakov.ExchangeRates.Server.Domain.Interfaces;

namespace yakov.ExchangeRates.Server.Infrastructure.FileServices
{
    public class SavedRatesService : ISavedRatesLoaderService
    {
        private IRatesFileService _ratesFileService;
        
        public Task LoadAll()
        {
            throw new NotImplementedException();
        }

        public Task SaveAll()
        {
            throw new NotImplementedException();
        }
    }
}
