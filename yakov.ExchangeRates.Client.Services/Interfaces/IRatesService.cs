using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using yakov.ExchangeRates.Client.Business;

namespace yakov.ExchangeRates.Client.Services.Interfaces
{
    public interface IRatesService
    {
        /// <summary>
        /// Get all currencies by type
        /// </summary>
        /// <param name="currencyType">Type of currency, which is fiat or crypto</param>
        /// <returns>List of currencies</returns>
        public Task<List<Currency>> GetCurrencies(CurrencyType currencyType);

        /// <summary>
        /// Get rates of currency with specified time period
        /// Longest period is 1 year
        /// Earliest time border is 5 years from current date
        /// </summary>
        /// <param name="currency">Currency to search rates</param>
        /// <param name="dateStart">Left time border (include)</param>
        /// <param name="dateEnd">Right time border (include)</param>
        /// <returns>Sorted by date rates list</returns>
        public Task<List<Rate>> GetRates(Currency currency, DateOnly dateStart, DateOnly dateEnd);
    }
}
