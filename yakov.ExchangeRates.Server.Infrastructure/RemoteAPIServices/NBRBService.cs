using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<List<Currency>> GetAllCurrencies<Currency>()
        {
            List<Currency> resultCurrencies = new(); 
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("api/exrates/currencies");
                if (response.IsSuccessStatusCode)
                {
                    var receivedCurrencies = await response.Content.ReadFromJsonAsync<List<Currency>>(); 
                    resultCurrencies.AddRange(receivedCurrencies);
                }
            }
            catch { }

            return resultCurrencies;
        }

        public async Task<List<Rate>> GetRatesByTimePeriod<Currency, Rate>(Currency currency, DateOnly dateStart, DateOnly dateEnd)
        {
            List<Rate> resultRates = new();
            CurrencyNBRB? currencyNBRB = currency as CurrencyNBRB;
            try
            {
                string arguments = $"{currencyNBRB?.Cur_ID}" +
                    $"?startDate={dateStart.ToString("yyyy-M-d")}" +
                    $"&endDate={dateEnd.ToString("yyyy-M-d")}";

                HttpResponseMessage response = await _httpClient.GetAsync("API/ExRates/Rates/Dynamics/" + arguments);
                if (response.IsSuccessStatusCode)
                {
                    var receivedRates = await response.Content.ReadFromJsonAsync<List<Rate>>();
                    resultRates.AddRange(receivedRates);
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
