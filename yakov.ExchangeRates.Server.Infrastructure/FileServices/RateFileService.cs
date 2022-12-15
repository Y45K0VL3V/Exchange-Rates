using System.Text.Json;
using yakov.ExchangeRates.Server.Domain.Entities;
using yakov.ExchangeRates.Server.Domain.Interfaces;

namespace yakov.ExchangeRates.Server.Infrastructure
{
    public class RateFileService : IRatesFileService
    {
        private IStorageService _storageService;

        private string GetPathByCurrency(Currency currency)
        {
            var subfolder = currency.Type.ToString();
            return $"{subfolder}\\{currency.ShortName}.json";
        }

        public async Task<string> GetSavedRatesByCurrency(Currency currency)
        {
            try
            {
                return await File.ReadAllTextAsync(GetPathByCurrency(currency));
            }
            catch { throw; }
        }

        public async Task WriteRates(IEnumerable<Rate> rates)
        {
            if (rates?.Count() == 0)
                return;

            try
            {
                string path = GetPathByCurrency(rates!.First().Currency);
                _storageService.CreateFile(path);

                await File.AppendAllTextAsync(path, JsonSerializer.Serialize(rates));
            }
            catch { throw; }
        }
    }
}
