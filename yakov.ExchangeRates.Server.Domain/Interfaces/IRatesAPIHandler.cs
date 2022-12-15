using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yakov.ExchangeRates.Server.Domain.Interfaces
{
    public interface IRatesAPIHandler
    {
        public Task<List<Currency>> GetAllCurrencies<Currency>();
        public Task<List<Rate>> GetRatesByTimePeriod<Currency, Rate>(Currency currency, DateOnly dateStart, DateOnly dateEnd);
    }
}
