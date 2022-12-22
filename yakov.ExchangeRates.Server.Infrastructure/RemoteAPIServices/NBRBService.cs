using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using yakov.ExchangeRates.Server.Application.Mappers;
using yakov.ExchangeRates.Server.Domain.Entities;
using yakov.ExchangeRates.Server.Domain.Entities.RemoteAPIs;
using yakov.ExchangeRates.Server.Domain.Interfaces;

namespace yakov.ExchangeRates.Server.Infrastructure.RemoteAPIServices
{
    public class NBRBService : IRatesAPIService
    {
        public NBRBService()
        {
            _httpClient = new();
            _httpClient.BaseAddress = new Uri(_nbrbUri);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private readonly HttpClient _httpClient;
        private readonly string _nbrbUri = "https://www.nbrb.by/";

        private async Task<ShortRateNBRB?> GetEnhancedRateData(string abbreviation)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"api/exrates/rates/{abbreviation}?parammode=2");
                if (response.IsSuccessStatusCode)
                {
                    var rate = await response.Content.ReadFromJsonAsync<ShortRateNBRB>();
                    return rate;
                }
                else
                    return null;
            }
            catch 
            {
                return null;
            }
        }

        private async Task<int?> GetCurrencyId(string currencyAbbreviation)
        {
            return (await GetEnhancedRateData(currencyAbbreviation))?.Cur_ID;
        }

        // Scale means number of foreign currency for the rate we get
        private async Task<int?> GetRateScale(string currencyAbbreviation)
        {
            return (await GetEnhancedRateData(currencyAbbreviation))?.Cur_Scale;
        }

        public async Task<List<Currency>> GetAllCurrencies()
        {
            List<Currency> resultCurrencies = new(); 
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("api/exrates/currencies");
                if (response.IsSuccessStatusCode)
                {
                    var receivedCurrencies = await response.Content.ReadFromJsonAsync<List<CurrencyNBRB>>();
                    receivedCurrencies?.ForEach(c => resultCurrencies.Add(c.ToCurrency()));
                }
            }
            catch { }

            return resultCurrencies;
        }

        public async Task<List<Rate>> GetRatesByTimePeriod(Currency currency, DateOnly dateStart, DateOnly dateEnd)
        {
            List<Rate> resultRates = new();
            try
            {
                string arguments = $"{await GetCurrencyId(currency.ShortName)}" +
                    $"?startDate={dateStart.ToString("yyyy-M-d")}" +
                    $"&endDate={dateEnd.ToString("yyyy-M-d")}";

                HttpResponseMessage response = await _httpClient.GetAsync("API/ExRates/Rates/Dynamics/" + arguments);
                if (response.IsSuccessStatusCode)
                {
                    var receivedRates = await response.Content.ReadFromJsonAsync<List<ShortRateNBRB>>();
                    var rateScale = await GetRateScale(currency.ShortName);
                    receivedRates?.ForEach(r => resultRates.Add(r.ToRate(currency, rateScale.Value)));
                }
            }
            catch { }

            return resultRates;
        }

        ~NBRBService()
        {
            _httpClient.Dispose();
        }
    }
}
