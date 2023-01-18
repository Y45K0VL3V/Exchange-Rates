using Microsoft.AspNetCore.Mvc;
using yakov.ExchangeRates.Server.Domain.Entities;
using yakov.ExchangeRates.Server.Domain.Interfaces;

namespace yakov.ExchangeRates.Server.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatesController : ControllerBase
    {
        private readonly ILogger<RatesController> _logger;
        private readonly IAPIServiceBuilder _apiServiceBuilder;
        private readonly ICacheService _cacheService;

        public RatesController(ILogger<RatesController> logger, ICacheService cacheService, IAPIServiceBuilder apiServiceBuilder)
        {
            _logger = logger;
            _cacheService = cacheService;
            _apiServiceBuilder = apiServiceBuilder;
        }

        [HttpGet("currencies")]
        public IEnumerable<string> GetCurrencyNames(CurrencyType currencyType)
        {
            var apiService = _apiServiceBuilder.BuildAPIService(currencyType);
            var currencies = apiService.GetAllCurrencies();
            currencies.Wait();

            return currencies.Result.Select(c => c.ShortName).Distinct();

        }

        [HttpGet]
        public IEnumerable<Rate> Get([FromQuery]Currency currency, string dateStart, string dateEnd)
        {
            var ratesTask = _cacheService.GetRatesWithPeriod(currency, DateOnly.Parse(dateStart), DateOnly.Parse(dateEnd));

            if (ratesTask.Wait(500000))
                return ratesTask.Result;
            else
                return new List<Rate>();

        }
    }
}