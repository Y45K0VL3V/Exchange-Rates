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
        private readonly ICacheService _cacheService;

        public RatesController(ILogger<RatesController> logger, ICacheService cacheService)
        {
            _logger = logger;
            _cacheService = cacheService;
        }

        [HttpGet]
        public IEnumerable<Rate> Get([FromQuery]Currency currency, string dateStart, string dateEnd)
        {
            //Currency currency = new() { ShortName = currency.ShortName, Type = currency.Type };
            var ratesTask = _cacheService.GetRatesWithPeriod(currency, DateOnly.Parse(dateStart), DateOnly.Parse(dateEnd));

            if (ratesTask.Wait(500000))
                return ratesTask.Result;
            else
                return new List<Rate>() { new Rate(), new Rate(), new Rate()};

        }
    }
}