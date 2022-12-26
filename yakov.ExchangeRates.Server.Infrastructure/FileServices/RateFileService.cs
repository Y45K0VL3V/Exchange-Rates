using System.Text.Json;
using yakov.ExchangeRates.Server.Domain.Entities;
using yakov.ExchangeRates.Server.Domain.Interfaces;

namespace yakov.ExchangeRates.Server.Infrastructure
{
    public class RateFileService : IRatesFileService
    {
        public RateFileService(IStorageService storageService)
        {
            _storageService = storageService;
        }

        private IStorageService _storageService;

        #region Path-Currency translators
        private static string GetPathByCurrency(Currency currency)
        {
            var subfolder = currency.Type.ToString();
            return $"{subfolder}\\{currency.ShortName}.json";
        }

        private static Currency GetCurrencyByPath(string path)
        {
            Currency currency = new();
            path = path.Remove(path.Length - 5);
            currency.ShortName = path[(path.LastIndexOf('\\') + 1)..];
            path = path.Remove(path.LastIndexOf('\\'));
            currency.Type = (CurrencyType)Enum.Parse(typeof(CurrencyType), path[(path.LastIndexOf('\\')+1)..]);

            return currency;
        }
        #endregion

        public async Task<Dictionary<Currency, List<Rate>>> GetSavedRates()
        {
            ////TODO: Properly check currency path
            
            var rateFilePaths = _storageService.GetAllPaths().Where(p => p.EndsWith(".json"));
            Dictionary<Currency, List<Rate>> rates = new();

            foreach (var path in rateFilePaths)
            {
                var currency = GetCurrencyByPath(path);
                var currRates = await GetSavedRatesByCurrency(currency);
                if (currRates != null)
                    rates.Add(currency, currRates);
            }

            return rates;
        }

        public async Task<List<Rate>?> GetSavedRatesByCurrency(Currency currency)
        {
            try
            {
                var ratesJson = await _storageService.ReadFileTextAsync(GetPathByCurrency(currency));
                return JsonSerializer.Deserialize<List<Rate>?>(ratesJson);
            }
            catch 
            {
                return null;
            }
        }

        public async Task WriteRatesByCurrency(List<Rate> rates, Currency currency)
        {
            if (rates?.Count == 0)
                return;

            try
            {
                string path = GetPathByCurrency(currency);
                _storageService.CreateFile(path);
                await _storageService.WriteFileTextAsync(path, JsonSerializer.Serialize(rates));
            }
            catch { }
        }
    }
}
