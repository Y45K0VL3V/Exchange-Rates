using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using yakov.ExchangeRates.Server.Application.Mappers;
using yakov.ExchangeRates.Server.Domain.Entities;
using yakov.ExchangeRates.Server.Domain.Entities.RemoteAPIs.Coin;
using yakov.ExchangeRates.Server.Domain.Interfaces;

namespace yakov.ExchangeRates.Server.Infrastructure.RemoteAPIServices
{
    public class CoinService : IRatesAPIService
    {
        public CoinService()
        {
            _httpClient = new();
            _httpClient.BaseAddress = new Uri(_coinUri);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add("X-CoinAPI-Key", "989AA9E9-7104-446D-9E1E-C0D0377A0B00");
        }

        private readonly HttpClient _httpClient;
        private readonly string _coinUri = "https://rest.coinapi.io/";

        public async Task<List<Currency>> GetAllCurrencies()
        {
            List<Currency> resultCurrencies = new();
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("v1/assets");
                if (response.IsSuccessStatusCode)
                {
                    var receivedCurrencies = await response.Content.ReadFromJsonAsync<List<CurrencyCoin>>();
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
                string arguments = $"{currency.ShortName}/USD/history?period_id=1DAY" +
                    $"&time_start={dateStart:yyyy-MM-dd}T00:00:00" +
                    $"&time_end={dateEnd:yyyy-MM-dd}T00:00:00";

                HttpResponseMessage response = await _httpClient.GetAsync("v1/exchangerate/" + arguments);
                if (response.IsSuccessStatusCode)
                {
                    var receivedRates = await response.Content.ReadFromJsonAsync<List<RateCoin>>();
                    receivedRates?.ForEach(r => resultRates.Add(r.ToRate(currency, 1)));
                }
            }
            catch { }

            return resultRates;
        }

        ~CoinService()
        {
            _httpClient.Dispose();
        }
    }
}
