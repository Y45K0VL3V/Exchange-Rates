using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using yakov.ExchangeRates.Server.Application.Mappers;
using yakov.ExchangeRates.Server.Domain.Entities;
using yakov.ExchangeRates.Server.Domain.Entities.RemoteAPIs.NBRB;
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

        private async Task<RateNBRB?> GetEnhancedRateData(string abbreviation)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"api/exrates/rates/{abbreviation}?parammode=2");
                if (response.IsSuccessStatusCode)
                {
                    var rate = await response.Content.ReadFromJsonAsync<RateNBRB>();
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

        private async Task<CurrencyNBRB?> GetCurrencyByAbbreviation(string abbreviation)
        {
            var currId = (await GetEnhancedRateData(abbreviation))?.ID;

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"api/exrates/currencies/{currId}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<CurrencyNBRB>();
                }
            }
            catch { }

            return null;
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
                    foreach (var currencyNBRB in receivedCurrencies ?? new())
                    {
                        if (currencyNBRB.IdEndChangeDate >= DateTime.Now &&
                            currencyNBRB.PeriodicityType == PeriodicityType.Daily)
                            resultCurrencies.Add(currencyNBRB.ToCurrency());
                    }
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
                var currencyNBRB = await GetCurrencyByAbbreviation(currency.ShortName);
                List<string> arguments = new();

                if (dateStart.ToDateTime(new()) <= currencyNBRB.IdChangeDate)
                {
                    if (dateEnd.ToDateTime(new()) <= currencyNBRB.IdChangeDate)
                        arguments.Add($"{currencyNBRB.PrevID}" +
                                      $"?startDate={dateStart:yyyy-M-d}" +
                                      $"&endDate={dateEnd:yyyy-M-d}");
                    else
                    {
                        arguments.Add($"{currencyNBRB.PrevID}" +
                                      $"?startDate={dateStart:yyyy-M-d}" +
                                      $"&endDate={currencyNBRB.IdChangeDate:yyyy-M-d}");
                        arguments.Add($"{currencyNBRB.CurrID}" +
                                      $"?startDate={currencyNBRB.IdChangeDate.AddDays(1):yyyy-M-d}" +
                                      $"&endDate={dateEnd:yyyy-M-d}");
                    }

                }
                else
                    arguments.Add($"{currencyNBRB.CurrID}" +
                                      $"?startDate={dateStart:yyyy-M-d}" +
                                      $"&endDate={dateEnd:yyyy-M-d}");

                foreach (var argument in arguments)
                {
                    HttpResponseMessage response = await _httpClient.GetAsync("API/ExRates/Rates/Dynamics/" + argument);
                    if (response.IsSuccessStatusCode)
                    {
                        var receivedRates = await response.Content.ReadFromJsonAsync<List<RateNBRB>>();
                        receivedRates?.ForEach(r => resultRates.Add(r.ToRate(currency, currencyNBRB.CurrScale)));
                    }
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
