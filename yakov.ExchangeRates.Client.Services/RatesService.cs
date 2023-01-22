using System.Net.Http.Headers;
using System.Net.Http.Json;
using yakov.ExchangeRates.Client.Business;
using yakov.ExchangeRates.Client.Services.Interfaces;

namespace yakov.ExchangeRates.Client.Services
{
    public class RatesService : IRatesService
    {
        public RatesService(ITimePeriodValidator timePeriodValidator)
        {
            _httpClient = new();
            _httpClient.BaseAddress = new Uri(_serverUri);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _timePeriodValidator = timePeriodValidator;
        }

        private readonly HttpClient _httpClient;
        private readonly string _serverUri = "https://localhost:7038/";

        private ITimePeriodValidator _timePeriodValidator;

        public async Task<List<string>> GetCurrencyNames(CurrencyType currencyType)
        {
            List<string> currencies;
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"api/Rates/currencies?currencyType={currencyType}");
                if (response.IsSuccessStatusCode)
                {
                    currencies = new((await response.Content.ReadFromJsonAsync<IEnumerable<string>>()));
                    return currencies;
                }
                else
                    return new();
            }
            catch
            {
                return new();
            }
        }

        public async Task<List<Rate>> GetRates(Currency currency, DateOnly dateStart, DateOnly dateEnd)
        {
            try
            {
                _timePeriodValidator.Validate(dateStart, dateEnd);
            }
            catch 
            { 
                throw; 
            }

            List<Rate> rates;
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"api/Rates?Type={currency.Type}&ShortName={currency.ShortName}" +
                                                                          $"&dateStart={dateStart}" +
                                                                          $"&dateEnd={dateEnd}");
                if (response.IsSuccessStatusCode)
                {
                    rates = new(await response.Content.ReadFromJsonAsync<IEnumerable<Rate>>());
                    return rates;
                }
                else
                    return new();
            }
            catch
            {
                return new();
            }
        }
    }
}
