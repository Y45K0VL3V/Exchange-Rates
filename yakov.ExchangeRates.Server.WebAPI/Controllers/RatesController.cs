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
        public async Task<IEnumerable<Rate>> Get(Currency currency, DateOnly dateStart, DateOnly dateEnd)
        {
            return await _cacheService.GetRatesWithPeriod(currency, dateStart, dateEnd);
        }
    }
}